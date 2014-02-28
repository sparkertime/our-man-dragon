using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class ProgressTracker : MonoBehaviour {

	public static int OrksKilled { get { return killedOrks.Count; } }
	public static float FireBreathed { get; private set; }
	public static int SecondsOfPlay { get; private set; }
	public string gameOverScene;

	private static bool villagersHaveSpawned;
	private static HashSet<Ork> killedOrks;

	public static string FormattedPlaytime() {
		var minutes = SecondsOfPlay / 60;
		var seconds = SecondsOfPlay % 60;
		
		return String.Format("{0}:{1:D2}", minutes, seconds);
	}

	public static void LogFireBreathed(float time) {
		FireBreathed += time;
	}

	public static void LogOrkKilled(Ork ork) {
		killedOrks.Add(ork);
	}

	public static void LogVillagerSpawned() {
		//total hack.
		villagersHaveSpawned = true;
	}

	void Awake() {
		killedOrks = new HashSet<Ork>();
		Hut.ResetAll();
		Fort.ResetAll();
		Villager.ResetAll();
		Ork.ResetAll();
		SecondsOfPlay =  0;
		villagersHaveSpawned = false;
		FireBreathed = 0f;
	}

	void Update() {
		UpdatePlaytime();

		if(villagersHaveSpawned && Villager.AllVillagers().Count() == 0) {
			Application.LoadLevel(gameOverScene);
		}
	}

	void UpdatePlaytime() {
		SecondsOfPlay = (int)Time.timeSinceLevelLoad;
	}
}
