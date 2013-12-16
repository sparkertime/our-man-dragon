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

	// Use this for initialization
	void Start () {
		spawnRate = initialFortSpawnRate;
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
		var spawnLocation = RandomPosition(minRange, maxRange);

		var newFort = (GameObject)GameObject.Instantiate(fort);
		newFort.transform.position = new Vector3(
			spawnLocation.x + minRange,
			newFort.transform.position.y,
			spawnLocation.y + minRange
			);
	}

	//A bit of a cheat since it subtracts a square from a circle, not a circle
	Vector2 RandomPosition(float minRadius, float maxRadius) {
		var relativePosition = Random.insideUnitCircle * (maxRadius - minRadius);
		return new Vector2(
			(relativePosition.x < 0f ? relativePosition.x - minRadius : relativePosition.x + minRadius),
			(relativePosition.y < 0f ? relativePosition.y - minRadius : relativePosition.y + minRadius)
			);

	}

	void ReduceSpawnRate() {
		spawnRate = Mathf.Max(minimumSpawnRate, spawnRateSpeedupRatio * spawnRate);
	}
}
