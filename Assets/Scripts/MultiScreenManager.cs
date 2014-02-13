using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class MultiScreenManager : MonoBehaviour {

	public Texture2D[] screens;
	public string nextLevel;

	private Queue<Texture2D> screenQueue;
	private Texture2D currentScreen;

	void Awake() {
		screenQueue = new Queue<Texture2D>(screens);
		currentScreen = NextScreen();
	}

	void Update() {
		if(Input.anyKeyDown) {
			currentScreen = NextScreen();
			if(currentScreen == null) {
				Application.LoadLevel(nextLevel);
			}
		}
	}

	void OnGUI() {
		if(currentScreen == null) return;

		InCenteredArea(currentScreen.width, currentScreen.height, () => {
			GUI.DrawTexture(
				new Rect(0,0,currentScreen.width, currentScreen.height),
				currentScreen
			);
		});
	}

	protected void InCenteredArea(float width, float height, Action drawFn) {
		GUILayout.BeginArea(new Rect( (Screen.width - width) / 2f, (Screen.height - height) / 2f, width, height),
		                    GUI.skin.box);
		
		drawFn();
		
		GUILayout.EndArea();
	}

	Texture2D NextScreen() {
		if(screenQueue.Any()) {
			return screenQueue.Dequeue();
		} else {
			return null;
		}
	}
}
