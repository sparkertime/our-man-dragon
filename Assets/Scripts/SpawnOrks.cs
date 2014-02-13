using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnOrks : MonoBehaviour {
	public float spawnRate = 2f;
	public int numberInTribe = 10;
	public GameObject ork;
	public float maxRange = 3f;
	public float minRange = 1f;

	private List<GameObject> orks;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;
	private bool hasAttacked = false;
	private RandomSpawner spawner;

	// Use this for initialization
	void Start() {
		timeSinceLastSpawn = Random.value;
		orks = new List<GameObject>();
		spawner = new RandomSpawner(this.transform, minRange, maxRange);
	}
	
	// Update is called once per frame
	void Update() {
		if(orks.Count >= numberInTribe) {
			AttemptAttack();
		}
		else {
			AttemptSpawn();
		}
	}

	void AttemptAttack() {
		if(hasAttacked) {
			return;
		}

		hasAttacked = true;
		orks.ForEach((ork) => ork.SendMessage("StartAttacking"));
	}
	
	void AttemptSpawn() {
		timeSinceLastSpawn += Time.deltaTime;
		
		if(timeSinceLastSpawn < spawnRate || !canSpawn) {
			return;
		}
		
		orks.Add(spawner.Spawn(ork));
		timeSinceLastSpawn = 0;
	}

	void StopActivity() {
		if(canSpawn) {
			canSpawn = false;
			AttemptAttack();
		}
	}
}
