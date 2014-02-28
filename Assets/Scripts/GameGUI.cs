using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameGUI : MonoBehaviour {
	public GUISkin alertSkin;
	public float alertInterval = 0.5f;
	public float alertTime = 3.0f;

	private float width = 300f;
	private float height = 70f;
	private bool isAlerting;
	private float currentAlertTime;
	private int villagerCount;

	void Awake() {
		villagerCount = 0;
		currentAlertTime = 0f;
		isAlerting = false;
	}

	void Update() {
		UpdateVillagerCount();
	}

	void OnGUI() {
		GUILayout.BeginArea(
			new Rect((Screen.width - width) / 2.0f, 0, width, height), 
			GUI.skin.box
		);

		GUI.Label(
			new Rect(5,3,100,25),
			String.Format("Villagers: {0} / {1}", villagerCount, Hut.TotalVillagerCapacity()),
			CurrentCountSkin()
		);

		GUI.Label(new Rect(width - 150, 3, 150, 25), String.Format("Currently {0}", HutBuilding.CurrentStateDescriptor()));

		GUI.Label(
			new Rect(5,28,100,25),
			String.Format("Orcs: {0} / {1}", Ork.AllOrks().Count(), Fort.TotalOrkCapacity())
		);

		GUI.Label(
			new Rect(width - 150, 28, 150, 25),
			String.Format("Time until next fort: {0}s", SpawnForts.TimeUntilNextSpawn())
		);

		GUI.Label(
			new Rect((width / 2f) - 35, 50, 80, 25),
			String.Format("Time: {0}", ProgressTracker.FormattedPlaytime())
		);

		GUILayout.EndArea();
	}

	private void UpdateVillagerCount() {
		var newCount = Villager.AllVillagers().Count();

		if(newCount < villagerCount) {
			isAlerting = true;
			currentAlertTime = 0.0f;
		}

		villagerCount = newCount;

		if(isAlerting) {
			currentAlertTime += Time.deltaTime;
		}

		if(currentAlertTime >= alertTime) {
			isAlerting = false;
			currentAlertTime = 0.0f;
		}

	}

	public GUIStyle CurrentCountSkin() {
		if(!isAlerting) return GUI.skin.label;

		return ((int)(currentAlertTime / alertInterval)) % 2 == 0 ? alertSkin.label : GUI.skin.label;
	}
}
