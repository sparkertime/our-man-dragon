using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hut : MonoBehaviour {
	private static List<Hut> _all = new List<Hut>();

	public static IEnumerable<Hut> AllHuts() {
		return _all;
	}

	public static int TotalVillagerCapacity() {
		if(_all.Count < 1) return 0;

		return _all.Count * _all[0].numberInTribe;
	}

	public float spawnRate = 3f;
	public int numberInTribe = 5;
	public GameObject villager;
	public float minRange = 1f;
	public float maxRange = 2f;

	public event Action OnDeath;
	
	private List<GameObject> villagers;
	private RandomSpawner spawner;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;

	// Use this for initialization
	void Start() {
		_all.Add(this);
		timeSinceLastSpawn = UnityEngine.Random.value;
		villagers = new List<GameObject>();
		spawner = new RandomSpawner(this.transform, minRange, maxRange);
	}

	public bool AtCapacity() {
		return villagers.Count >= numberInTribe;
	}
	
	// Update is called once per frame
	void Update() {
		AttemptSpawn();
	}
	
	void AttemptSpawn() {
		timeSinceLastSpawn += Time.deltaTime;

		if(!canSpawn || AtCapacity() || timeSinceLastSpawn < spawnRate) {
			return;
		}

		timeSinceLastSpawn = 0f;
		
		villagers.Add(spawner.Spawn(villager));
	}

	void StopActivity() {
		canSpawn = false;
		_all.Remove(this);

		if(OnDeath != null) OnDeath();
	}
}