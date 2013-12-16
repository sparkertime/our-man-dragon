using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	private Villager target;
	private bool isAttacking = false;

	public float movementSpeed = 4f;
	public float killRadius = 0.3f;

	void StartAttacking() {
		isAttacking = true;
	}

	void Update() {
		if(isAttacking) {
			SelectTarget();
			MoveTowardsTarget();
			AttemptKill();
		}
	}

	void SelectTarget() {
		if(target != null && target.IsLiving() ) {
			return;
		}

		target = Villager.RandomVillager();
	}

	void MoveTowardsTarget() {
		if(!isAttacking || target == null || !target.IsLiving()) {
			return;
		}

		this.transform.LookAt(target.gameObject.transform.position);
		this.transform.position += this.transform.forward * Time.deltaTime * movementSpeed;
	}

	void AttemptKill() {
		if(!isAttacking || target == null || !target.IsLiving()) {
			return;
		}

		if(Vector3.Distance(target.gameObject.transform.position, this.transform.position) <= killRadius) {
			target.Kill();
			target = null;
		}
	}

	public void StopActivity() {
		isAttacking = false;
	}
}
