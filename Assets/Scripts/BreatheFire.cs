using UnityEngine;
using System.Collections;

public class BreatheFire : MonoBehaviour {

	public GameObject fire;

	// Use this for initialization
	void Start () {
		fire.GetComponent<ParticleSystem>().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		fire.GetComponent<ParticleSystem>().enableEmission = Input.GetButton("Fire");

		if(fire.GetComponent<ParticleSystem>().enableEmission) {
			ProgressTracker.LogFireBreathed(Time.deltaTime);
		}
	}
}
