using UnityEngine;
using System.Collections;

public class Burnable : MonoBehaviour {
	public GameObject fire;
	public GameObject smoulder;
	public int hitsUntilBurn = 3;
	public float timeUntilSmoulder = 4f;

	private int timesHit = 0;
	private float timeBurning = 0f;

	void Start() {
		this.fire.particleSystem.enableEmission = false;
		this.smoulder.particleSystem.enableEmission = false;
	}

	void OnParticleCollision(GameObject collision) {
		timesHit += 1;

		if(timesHit >= hitsUntilBurn) {
			Burn();
		}
	}

	void Update() {
		if(IsBurning()) {
			Burn();
		}
	}

	bool IsBurning() {
		return timeBurning > 0f;
	}

	void Burn() {
		timeBurning += Time.deltaTime;

		this.fire.particleSystem.enableEmission = (timeBurning < timeUntilSmoulder);
		this.smoulder.particleSystem.enableEmission = (timeBurning > timeUntilSmoulder);
	}
}
