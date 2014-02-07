using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class HutBuilding : MonoBehaviour {
	public GameObject hut;
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
	
	public BuildState currentState = BuildState.SpawningVillagers;
	private Dictionary<BuildState, Action> stateActions;
	private RandomSpawner spawner;
	public float hutBuildingProgress = 0f;
	private Vector2 nextLocation;

	// Use this for initialization
	void Start() {
		spawner = new RandomSpawner(this.transform, minRange, maxRange);

		stateActions = new Dictionary<BuildState, Action>{
			{BuildState.SpawningVillagers, WaitForCapacity},
			{BuildState.LookingForLocation, FindNextBuildSite},
			{BuildState.AssemblingBuilders, GatherBuilders},
			{BuildState.BuildingHut, BuildHut}
		};

		SpawnHut(0,0);
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
			Debug.Log(String.Format("Hut Built at {0}, {1}!", nextLocation.x, nextLocation.y));
			SpawnHut(nextLocation.x, nextLocation.y);
			builders.Clear();
			currentState = BuildState.SpawningVillagers;
		}
	}

	void SpawnHut(float x, float y) {
		var newHut = spawner.Spawn(hut, new Vector2(x, y)).GetComponent<Hut>();
		newHut.OnDeath += () => currentState = BuildState.SpawningVillagers;
	}
}