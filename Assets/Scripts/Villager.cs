using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Villager : MonoBehaviour {
	enum VillagerState {
		Idle = 0,
		Moving,
		Dead
	}
	
	private static List<Villager> _all = new List<Villager>();
	public static IEnumerable<Villager> AllVillagers() {
		return _all;
	}

	public float hutBuildDistance = 0.8f;
	public float movementSpeed = 2f;
	public event Action<Villager> OnDeath, OnArrive;
	
	static System.Random randomNumber = new System.Random();
	VillagerState state = VillagerState.Idle;

	private Vector3 destination;

	public static Villager RandomVillager() {
		return _all[randomNumber.Next(0, _all.Count)];
	}
	
	public static Villager RandomIdleVillager() {
		return _all
			.Where(v => v.state == VillagerState.Idle)
			.OrderBy(v => randomNumber.NextDouble())
			.FirstOrDefault();
	}
	
	void Start () {
		_all.Add(this);
	}

	public bool IsLiving() {
		return state != VillagerState.Dead;
	}

	public bool IsIdle() {
		return state == VillagerState.Idle;
	}

	public void Kill() {
		state = VillagerState.Dead;

		if(OnDeath != null) OnDeath(this);
		_all.Remove(this);

		Destroy(this.gameObject);
	}
	
	public void SetDestinationNear(Vector2 destination) {
		var nextLocation = new RandomSpawner(destination, hutBuildDistance - 0.1f, hutBuildDistance + 0.1f).NextLocation();

		this.destination = new Vector3(nextLocation.x,
		                               this.transform.position.y,
		                               nextLocation.y);

		state = VillagerState.Moving;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == VillagerState.Moving) {
			MoveTowardsDestination();
		}
	}

	void MoveTowardsDestination() {
		if(Vector3.Distance(this.transform.position, destination) < 0.1f) {
			state = VillagerState.Idle;

			if(OnArrive != null) OnArrive(this);

			return;
		}

		this.transform.LookAt(destination);
		this.transform.position += this.transform.forward * Time.deltaTime * movementSpeed;
	}
}
