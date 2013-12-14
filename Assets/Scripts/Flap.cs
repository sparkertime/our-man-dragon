using UnityEngine;
using System.Collections;

public class Flap : MonoBehaviour {

	public float maxAngle = 20.0f;
	public float flapDuration = 1.5f;

	// Update is called once per frame
	void Update () {
		float wingPos = Time.time / flapDuration * 2f * Mathf.PI;
		this.transform.localEulerAngles = new Vector3(
			(1.0f - Mathf.Cos(wingPos)) * maxAngle,
			this.transform.rotation.y, 
			this.transform.rotation.z);
	}
}
