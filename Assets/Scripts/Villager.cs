using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Villager : MonoBehaviour {
	enum VillagerState {
		Idle = 0,
		MovingToBuilding,
		Building,
		Wandering,
		Dead
	}
	
	private static List<Villager> _all = new List<Villager>();
	public static IEnumerable<Villager> AllVillagers() {
		return _all;
	}

	public static void ResetAll() {
		_all = new List<Villager>();
	}

	public float hutBuildDistance = 0.8f;
	public float movementSpeed = 2f;
	public float wanderOdds = 0.333f;
	public float wanderingSpeed = 0.4f;
	public float wanderingRadius = 1f;
	public event Action<Villager> OnDeath, OnArrive;
	
	static System.Random randomNumber = new System.Random();
	VillagerState state = VillagerState.Idle;

	private Vector3 buildDestination;
	private Vector3 wanderDestination;
	private int lastWanderCheck;

	public static Villager RandomVillager() {
		if(_all.Count < 1) return null;

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
		ProgressTracker.LogVillagerSpawned();
	}

	public bool IsLiving() {
		return state != VillagerState.Dead;
	}

	public bool IsBuilding() {
		return state == VillagerState.Building;
	}

	public void Idle() {
		state = VillagerState.Idle;
	}

	public void Kill() {
		StopActivity();

		Destroy(this.gameObject);
	}
	
	public void Build(Vector2 destination) {
		var nextLocation = new RandomSpawner(destination, hutBuildDistance - 0.1f, hutBuildDistance + 0.1f).NextLocation();

		this.buildDestination = new Vector3(nextLocation.x,
           this.transform.position.y,
           nextLocation.y
		);

		state = VillagerState.MovingToBuilding;
	}

	void CheckForWandering() {
		var second = (int)Time.timeSinceLevelLoad;

		if(second != lastWanderCheck && UnityEngine.Random.value < wanderOdds) {
			var nextLocation = RandomSpawner.LocationNear(
				new Vector2(this.transform.position.x,this.transform.position.z),
				wanderingRadius
			);

			wanderDestination = new Vector3(nextLocation.x,
	            this.transform.position.y,
	            nextLocation.y
			);

			this.state = VillagerState.Wandering;
		}

		lastWanderCheck = second;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == VillagerState.MovingToBuilding) {
			MoveTowardsBuilding();
		}
		else if (state == VillagerState.Wandering) {
			MoveWandering();
		}
		else if(state == VillagerState.Idle) {
			CheckForWandering();
		}
	}

	void MoveTowardsBuilding() {
		if(Vector3.Distance(this.transform.position, buildDestination) < 0.1f) {
			state = VillagerState.Building;

			if(OnArrive != null) OnArrive(this);

			OnArrive = null;

			return;
		}

		this.transform.LookAt(buildDestination);
		this.transform.position += this.transform.forward * Time.deltaTime * movementSpeed;
	}
	
	void MoveWandering() {
		if(Vector3.Distance(this.transform.position, wanderDestination) < 0.1f) {
			state = VillagerState.Idle;
			
			return;
		}
		
		this.transform.LookAt(wanderDestination);
		this.transform.position += this.transform.forward * Time.deltaTime * wanderingSpeed;

		lastWanderCheck = (int)Time.timeSinceLevelLoad;
	}

	void StopActivity() {
		state = VillagerState.Dead;
		
		if(OnDeath != null) OnDeath(this);
		_all.Remove(this);
	}
}
