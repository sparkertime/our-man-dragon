using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public GameObject target;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = target.transform.InverseTransformPoint(this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = target.transform.TransformPoint(offset);
		this.transform.eulerAngles = new Vector3(
			this.transform.eulerAngles.x,
			target.transform.eulerAngles.y,
			this.transform.eulerAngles.z);
	}
}
