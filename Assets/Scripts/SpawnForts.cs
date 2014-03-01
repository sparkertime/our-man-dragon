using UnityEngine;
using System.Collections;

public class SpawnForts : MonoBehaviour {

	public static int TimeUntilNextSpawn() {
		return theInstance._timeUntilNextSpawn();
	}

	private static SpawnForts theInstance;

	public float initialFortSpawnRate = 30f;
	public float minimumSpawnRate = 4f;
	public float spawnRateSpeedupRatio = 0.8f;
	public float maxRange = 45f;
	public float minRange = 20f;
	public GameObject fort;

	private float timeSinceLastSpawn = 0f;
	private float spawnRate;
	private RandomSpawner spawner;

	// Use this for initialization
	void Start () {
		theInstance = this;
		timeSinceLastSpawn = Random.value;
		spawnRate = initialFortSpawnRate;
		spawner = new RandomSpawner(this.transform, minRange, maxRange);
		SpawnInitialFort();
	}
	
	// Update is called once per frame
	void Update () {
		AttemptSpawn();
	}

	void AttemptSpawn() {
		timeSinceLastSpawn += Time.deltaTime;

		if(timeSinceLastSpawn < spawnRate) {
			return;
		}

		spawner.Spawn(fort);
		timeSinceLastSpawn = 0;
		ReduceSpawnRate();
	}

	void ReduceSpawnRate() {
		spawnRate = Mathf.Max(minimumSpawnRate, spawnRateSpeedupRatio * spawnRate);
	}

	int _timeUntilNextSpawn() {
		return (int)(spawnRate - timeSinceLastSpawn);
	}

	void SpawnInitialFort() {
		var newFort = spawner.Spawn(fort);

		newFort.transform.position = new Vector3(
			newFort.transform.position.x,
			newFort.transform.position.y,
			Mathf.Abs(newFort.transform.position.z)
		);
	}
}
