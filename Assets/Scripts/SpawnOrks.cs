using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnOrks : MonoBehaviour {

	public float spawnInterval = 5f;
	public int numberInTribe = 15;
	public GameObject ork;
	public float spawnRadius = 10f;

	private List<GameObject> orks;
	private float timeSinceLastSpawn = 0f;

	// Use this for initialization
	void Start() {
		orks = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update() {
		SpawnOrk();
	}

	void SpawnOrk() {
		timeSinceLastSpawn += Time.deltaTime;
		if(timeSinceLastSpawn > spawnInterval) {
			timeSinceLastSpawn = 0f;

			var newOrk = (GameObject)GameObject.Instantiate(ork);
			var newPositionOffset = (Random.insideUnitCircle * spawnRadius);

			newOrk.transform.position = new Vector3(
				this.transform.position.x + newPositionOffset.x,
				newOrk.transform.position.y,
				newOrk.transform.position.z + newPositionOffset.y);
			orks.Add(newOrk);
		}
	}
}
