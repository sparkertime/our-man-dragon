using UnityEngine;
using System.Collections;

public class Flight : MonoBehaviour {
	public float speed = 1.0f;

	// this is very stupid, but a side effect of making my "forward" the local x axis on Dragon, not z like a grownup. 
	public Vector3 forward = Vector3.forward;
	
	// Update is called once per frame
	void Update() {
		this.transform.position += forward * Time.deltaTime * speed;
	}
}
