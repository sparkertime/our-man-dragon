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

		if(timeBurning > timeUntilDeath) {
			Destroy(destroyOnBurn);
		} else if(timeBurning > timeUntilSmoulder) {
			this.fire.particleSystem.enableEmission = false;
			this.smoulder.particleSystem.enableEmission = true;
			foreach(var obj in objectsToScorch) {
				obj.renderer.materials = Enumerable.Repeat<Material>(scorchColor, obj.renderer.materials.Count()).ToArray();
			}
		}
	}

	bool IsBurning() {
		return timeBurning > 0f;
	}
}
