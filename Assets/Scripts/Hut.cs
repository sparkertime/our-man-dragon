using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hut : MonoBehaviour {
	
	public float spawnInterval = 3f;
	public int numberInTribe = 5;
	public GameObject villager;
	public float minSpawnRange = 0.5f;
	public float maxSpawnRange = 1.5f;
	
	private List<GameObject> villagers;
	private RandomSpawnLocation spawnLocation;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;
	
	// Use this for initialization
	void Start() {
		timeSinceLastSpawn = Random.value;
		villagers = new List<GameObject>();
		spawnLocation = new RandomSpawnLocation(this.transform, minSpawnRange, maxSpawnRange);
	}

	public bool HasCapacity() {
		return villagers.Count < numberInTribe;
	}
	
	// Update is called once per frame
	void Update() {
		if(HasCapacity()) {
			SpawnVillager();
		}
	}
	
	void SpawnVillager() {
		if(!canSpawn) {
			return;
		}

		timeSinceLastSpawn += Time.deltaTime;
		if(timeSinceLastSpawn > spawnInterval) {
			timeSinceLastSpawn = 0f;
			
			var newVillager = (GameObject)GameObject.Instantiate(villager);
			var location = spawnLocation.Next();
			
			newVillager.transform.position = new Vector3(
				location.x,
				newVillager.transform.position.y,
				location.z
				);
			
			villagers.Add(newVillager);
		}
	}

	void StopActivity() {
		canSpawn = false;
	}
}
