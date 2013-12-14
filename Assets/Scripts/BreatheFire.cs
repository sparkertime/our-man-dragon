using UnityEngine;
using System.Collections;

public class BreatheFire : MonoBehaviour {

	public GameObject fire;

	// Use this for initialization
	void Start () {
		fire.particleSystem.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		fire.particleSystem.enableEmission = Input.GetButton("Fire");
	}
}
