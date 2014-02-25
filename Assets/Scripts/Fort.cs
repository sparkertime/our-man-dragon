using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fort : MonoBehaviour {

	private static List<Fort> _all = new List<Fort>();

	public static int TotalOrkCapacity() {
		if(_all.Count < 1) return 0;
		
		return _all.Count * _all[0].numberInTribe;
	}

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
		_all.Add(this);
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

		_all.Remove(this);
	}
}
