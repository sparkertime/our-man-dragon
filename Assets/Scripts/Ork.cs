using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ork : MonoBehaviour {

	private static List<Ork> _all = new List<Ork>();
	public static IEnumerable<Ork> AllOrks() {
		return _all;
	}
	
	public static void ResetAll() {
		_all = new List<Ork>();
	}

	private Villager target;
	private bool canAttack = true;
	private bool isAttacking = false;

	public float movementSpeed = 4f;
	public float killRadius = 0.3f;

	void Start() {
		_all.Add(this);
	}

	void StartAttacking() {
		if(canAttack) {
			isAttacking = true;
		}
	}

	void Update() {
		if(canAttack && isAttacking) {
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
		if(target == null || !target.IsLiving()) {
			return;
		}

		this.transform.LookAt(target.gameObject.transform.position);
		this.transform.position += this.transform.forward * Time.deltaTime * movementSpeed;
	}

	void AttemptKill() {
		if(target == null || !target.IsLiving()) {
			return;
		}

		if(Vector3.Distance(target.gameObject.transform.position, this.transform.position) <= killRadius) {
			target.Kill();
			target = null;
		}
	}

	public void StopActivity() {
		canAttack = false;
		ProgressTracker.LogOrkKilled(this);
		_all.Remove(this);
	}
}
