using UnityEngine;
using System.Collections;

public class SpawnForts : MonoBehaviour {
	public float initialFortSpawnRate = 20f;
	public float minimumSpawnRate = 5f;
	public float spawnRateSpeedupRatio = 0.8f;
	public float maxRange = 40f;
	public float minRange = 5f;
	public GameObject fort;

	private float timeSinceLastSpawn = 0f;
	private float spawnRate;
	private RandomSpawnLocation spawnLocation;

	// Use this for initialization
	void Start () {
		timeSinceLastSpawn = Random.value;
		spawnRate = initialFortSpawnRate;
		spawnLocation = new RandomSpawnLocation(this.transform, minRange, maxRange);
		SpawnFort();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastSpawn += Time.deltaTime;

		if(timeSinceLastSpawn > spawnRate) {
			SpawnFort();
			ReduceSpawnRate();
		}
	}

	void SpawnFort() {
		timeSinceLastSpawn = 0f;
		var location = spawnLocation.Next();

		var newFort = (GameObject)GameObject.Instantiate(fort);
		newFort.transform.position = new Vector3(
			location.x,
			newFort.transform.position.y,
			location.z
			);
	}

	void ReduceSpawnRate() {
		spawnRate = Mathf.Max(minimumSpawnRate, spawnRateSpeedupRatio * spawnRate);
	}
}
