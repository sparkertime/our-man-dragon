using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class HutBuilding : MonoBehaviour {
	public GameObject hut;
	public GameObject underConstructionHut;
	public int startingAlpha;
	public int finalAlpha;
	public float maxRange = 10f;
	public float minRange = 1.0f;
	public float buildTimeForHut = 15f;
	public float minimumHutSpace = 2f;
	public int villagersToBuild = 3;

	public enum BuildState {
		SpawningVillagers = 0,
		LookingForLocation,
		AssemblingBuilders,
		BuildingHut
	}

	public static Dictionary<BuildState, String> buildStateDescriptors = new Dictionary<BuildState, string> {
		{BuildState.SpawningVillagers, "making babies"},
		{BuildState.LookingForLocation, "building a hut"},
		{BuildState.AssemblingBuilders, "building a hut"},
		{BuildState.BuildingHut, "building a hut"}
	};

	private static HutBuilding theInstance;
	private BuildState currentState = BuildState.SpawningVillagers;
	private Dictionary<BuildState, Action> stateActions;
	private RandomSpawner spawner;
	private float hutBuildingProgress = 0f;
	private Vector2 nextLocation;
	private GameObject spawnedUnderConstructionHut;

	public static string CurrentStateDescriptor() {
		if(theInstance == null) return String.Empty;

		return buildStateDescriptors[theInstance.currentState];
	}

	// Use this for initialization
	void Start() {
		theInstance = this;
		spawner = new RandomSpawner(this.transform, minRange, maxRange);

		stateActions = new Dictionary<BuildState, Action>{
			{BuildState.SpawningVillagers, WaitForCapacity},
			{BuildState.LookingForLocation, FindNextBuildSite},
			{BuildState.AssemblingBuilders, GatherBuilders},
			{BuildState.BuildingHut, BuildHut}
		};

		SpawnHut(new Vector2(0,0));
	}

	void FixedUpdate() {
		stateActions[currentState].Invoke();
	}

	void WaitForCapacity() {
		if(Villager.AllVillagers().Count() > villagersToBuild && Hut.AllHuts().All((h) => h.AtCapacity())) {
			currentState = BuildState.LookingForLocation;
		}
	}

	void FindNextBuildSite() {
		var location2D = spawner.NextLocation();
		var location3D = new Vector3(location2D.x, hut.transform.position.y, location2D.y);

		if(Hut.AllHuts().All(h => Vector3.Distance(h.transform.position, location3D) >= minimumHutSpace)) {
			nextLocation = location2D;
			currentState = BuildState.AssemblingBuilders;
		}
	}
	
	private List<Villager> builders = new List<Villager>();
	void GatherBuilders() {
		var buildersToAdd = villagersToBuild - builders.Count;
		for(int i = 0; i < buildersToAdd; i++) {
			var villager = Villager.RandomIdleVillager();

			if (villager == null) return;

			villager.OnDeath += RemoveDeadBuilder;
			villager.OnArrive += CheckForBuildingStart;
			villager.SetDestinationNear(nextLocation);
			builders.Add(villager);
		}
	}

	void RemoveDeadBuilder(Villager deadVillager) {
		builders.Remove(deadVillager);
		currentState = BuildState.AssemblingBuilders;
		hutBuildingProgress = 0f;

		if(spawnedUnderConstructionHut) {
			Destroy(spawnedUnderConstructionHut);
		}
	}

	void CheckForBuildingStart(Villager _) {
		if(builders.All(v => v.IsIdle())) {
			hutBuildingProgress = 0;
			currentState = BuildState.BuildingHut;
		}
	}

	void BuildHut() {
		hutBuildingProgress += Time.fixedDeltaTime;
		if(hutBuildingProgress >= buildTimeForHut) {
			SpawnHut(nextLocation);
			builders.Clear();
			currentState = BuildState.SpawningVillagers;
		} else {
			UpdateConstructionProgress();
		}
	}

	void UpdateConstructionProgress() {
		if(spawnedUnderConstructionHut == null) {
			spawnedUnderConstructionHut = spawner.Spawn(underConstructionHut, nextLocation);
		}
		foreach(var renderer in spawnedUnderConstructionHut.GetComponentsInChildren<Renderer>()) {
			renderer.material.color = new Color(
				renderer.material.color.r,
				renderer.material.color.g,
				renderer.material.color.b,
				Mathf.Lerp(0.1f, 0.8f, hutBuildingProgress / buildTimeForHut)
			);
		}
	}

	void SpawnHut(Vector2 location) {
		if(spawnedUnderConstructionHut != null) {
			Destroy(spawnedUnderConstructionHut);
			spawnedUnderConstructionHut = null;
		}
		var newHut = spawner.Spawn(hut, location).GetComponent<Hut>();
		newHut.OnDeath += () => currentState = BuildState.SpawningVillagers;
	}
}