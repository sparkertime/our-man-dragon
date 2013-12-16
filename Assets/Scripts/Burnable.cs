using UnityEngine;
using System.Collections;
using System;

public class Burnable : MonoBehaviour {
	public GameObject fire;
	public GameObject smoulder;
	public float timeUntilSmoulder = 2f;
	public float timeFromSmoulderTillDeath = 2f;

	private float timeBurning = 0f;

	void Start() {
		this.fire.particleSystem.enableEmission = false;
		this.smoulder.particleSystem.enableEmission = false;
	}

	void OnParticleCollision(GameObject collision) {
		Burn();
		this.SendMessageUpwards("StopActivity", SendMessageOptions.DontRequireReceiver);
	}

	void Update() {
		if(IsBurning()) {
			Burn();
		}
	}

	void Burn() {
		timeBurning += Time.deltaTime;
		
		this.fire.particleSystem.enableEmission = (timeBurning < timeUntilSmoulder);
		this.smoulder.particleSystem.enableEmission = (timeBurning > timeUntilSmoulder);

		if(timeBurning > (timeUntilSmoulder + timeFromSmoulderTillDeath)) {
			Destroy(this.gameObject);
		}
	}

	bool IsBurning() {
		return timeBurning > 0f;
	}
}
