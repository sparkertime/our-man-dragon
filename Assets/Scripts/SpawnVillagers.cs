using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnVillagers : MonoBehaviour {
	
	public float spawnInterval = 3f;
	public int numberInTribe = 5;
	public GameObject villager;
	public float spawnRadius = 10f;
	
	private List<GameObject> villagers;
	private float timeSinceLastSpawn = 0f;
	private bool canSpawn = true;
	
	// Use this for initialization
	void Start() {
		villagers = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update() {
		if(villagers.Count < numberInTribe) {
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
			var newPositionOffset = (Random.insideUnitCircle * spawnRadius);
			
			newVillager.transform.position = new Vector3(
				this.transform.position.x + newPositionOffset.x,
				newVillager.transform.position.y,
				this.transform.position.z + newPositionOffset.y);
			
			villagers.Add(newVillager);
		}
	}

	void StopActivity() {
		canSpawn = false;
	}
}
