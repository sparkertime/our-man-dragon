using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hut : MonoBehaviour {
	private static List<Hut> _all = new List<Hut>();

	public static IEnumerable<Hut> AllHuts() {
		return _all;
	}

	public static Hut RandomHut() {
		if(_all.Count == 0) return null;

		return _all[UnityEngine.Random.Range(0, _all.Count)];
	}

	public static void ResetAll() {
		_all = new List<Hut>();
	}

	public float spawnRate = 3f;
	public GameObject villager;
	public float minRange = 1f;
	public float maxRange = 2f;

	public event Action OnDeath;

	private RandomSpawner spawner;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;

	public void SpawnVillager() {
		spawner.Spawn(villager);
	}

	// Use this for initialization
	void Start() {
		_all.Add(this);
		timeSinceLastSpawn = UnityEngine.Random.value;
		spawner = new RandomSpawner(this.transform, minRange, maxRange);
	}
	
	// Update is called once per frame
	void Update() {
		AttemptSpawn();
	}
	
	void AttemptSpawn() {
		timeSinceLastSpawn += Time.deltaTime;

		if(!canSpawn || timeSinceLastSpawn < spawnRate) {
			return;
		}

		SpawnVillagers.AttemptSpawnVillager();

		timeSinceLastSpawn = 0f;
	}

	void StopActivity() {
		canSpawn = false;
		_all.Remove(this);

		if(OnDeath != null) OnDeath();
	}
}