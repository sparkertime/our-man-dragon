using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hut : MonoBehaviour {
	
	public float spawnRate = 3f;
	public int numberInTribe = 5;
	public GameObject villager;
	public float minRange = 1f;
	public float maxRange = 2f;
	
	private List<GameObject> villagers;
	private RandomSpawner spawner;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;
	
	// Use this for initialization
	void Start() {
		timeSinceLastSpawn = Random.value;
		villagers = new List<GameObject>();
		spawner = new RandomSpawner(this.transform, minRange, maxRange);
	}

	public bool HasCapacity() {
		return villagers.Count < numberInTribe;
	}
	
	// Update is called once per frame
	void Update() {
		AttemptSpawn();
	}
	
	void AttemptSpawn() {
		timeSinceLastSpawn += Time.deltaTime;

		if(!canSpawn || !HasCapacity() || timeSinceLastSpawn < spawnRate) {
			return;
		}

		timeSinceLastSpawn = 0f;
		
		villagers.Add(spawner.Spawn(villager));
	}

	void StopActivity() {
		canSpawn = false;
	}
}
