using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameGUI : MonoBehaviour {
	//what's important to know
	//villagers count/max & state
	//flash red when dying
	//orks count
	//forts count & timer
	//game time

	private float width = 300f;
	private float height = 60f;

	void OnGUI() {
		GUILayout.BeginArea(
			new Rect((Screen.width - width) / 2.0f, 0, width, height), 
			GUI.skin.box
		);

		GUI.Label(new Rect(0,0,100,30), String.Format("Villagers: {0} / {1}", Villager.AllVillagers().Count(), Hut.TotalVillagerCapacity()));

		GUILayout.EndArea();
	}
}
