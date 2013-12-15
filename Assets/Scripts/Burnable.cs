using UnityEngine;
using System.Collections;
using System;

public class Burnable : MonoBehaviour {
	public GameObject fire;
	public GameObject smoulder;
	public float timeUntilSmoulder = 4f;
	private float timeBurning = 0f;

	void Start() {
		this.fire.particleSystem.enableEmission = false;
		this.smoulder.particleSystem.enableEmission = false;
	}

	void OnParticleCollision(GameObject collision) {
		Burn();
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
	}

	bool IsBurning() {
		return timeBurning > 0f;
	}
}
