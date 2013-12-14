using UnityEngine;
using System.Collections;

public class Flight : MonoBehaviour {
	public float forwardSpeed = 4.0f;
	public float turnRate = 15f;
	
	// Update is called once per frame
	void Update() {
		if(Input.GetAxis("Vertical") > 0.1f) {
			this.transform.position += this.transform.forward * Time.deltaTime * forwardSpeed * Input.GetAxis("Vertical");
		}
		if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
			this.transform.RotateAround(this.transform.position, Vector3.up, turnRate * Input.GetAxis("Horizontal") *  Time.deltaTime);
		}
	}
}
