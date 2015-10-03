using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Burnable : MonoBehaviour {
	public GameObject fire;
	public GameObject smoulder;
	public GameObject destroyOnBurn;
	public GameObject[] objectsToScorch;
	public Material scorchColor;
	public static float timeUntilSmoulder = 4f;
	public static float timeUntilDeath = 8f;

	private float timeBurning = 0f;

	void Start() {
		this.fire.GetComponent<ParticleSystem>().enableEmission = false;
		this.smoulder.GetComponent<ParticleSystem>().enableEmission = false;
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
		
		this.fire.GetComponent<ParticleSystem>().enableEmission = (timeBurning < timeUntilSmoulder);
		this.smoulder.GetComponent<ParticleSystem>().enableEmission = (timeBurning > timeUntilSmoulder);

		if(timeBurning > timeUntilDeath) {
			destroyOnBurn.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
			Destroy(destroyOnBurn);
		} else if(timeBurning > timeUntilSmoulder) {
			this.fire.GetComponent<ParticleSystem>().enableEmission = false;
			this.smoulder.GetComponent<ParticleSystem>().enableEmission = true;
			MakeScorched();
		}
	}

	private bool _isScorched = false;
	void MakeScorched() {
		if(_isScorched) {
			return;
		}

		foreach(var obj in objectsToScorch) {
			obj.GetComponent<Renderer>().materials = Enumerable.Repeat<Material>(scorchColor, obj.GetComponent<Renderer>().materials.Count()).ToArray();
		}
		_isScorched = true;
	}

	bool IsBurning() {
		return timeBurning > 0f;
	}
}
