using UnityEngine;
using System.Collections;

public class Burnable : MonoBehaviour {

	void OnParticleCollision(GameObject collision) {
		Debug.Log("Something hit me! Fire?");
	}
}
