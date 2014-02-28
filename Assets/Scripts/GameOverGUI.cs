using UnityEngine;
using System.Collections;
using System;

public class GameOverGUI : MonoBehaviour {
	
	public string nextLevel;
	public Texture2D gameOverScreen;
	public GUIStyle textStyle;

	void Update() {
		if(Input.anyKeyDown) {
			Application.LoadLevel(nextLevel);
		}
	}

	void OnGUI() {
		InCenteredArea(gameOverScreen.width, gameOverScreen.height, () => {
			GUI.DrawTexture(
				new Rect(0,0,gameOverScreen.width, gameOverScreen.height),
				gameOverScreen
			);

			GUI.Label(
				new Rect(252,250, 300, 50),
				String.Format("Your Time: {0}", ProgressTracker.FormattedPlaytime()),
				textStyle
			);

			GUI.Label(
				new Rect(220,300, 300, 50),
				String.Format("Orcs Burned: {0}", ProgressTracker.OrksKilled),
				textStyle
			);

			GUI.Label(
				new Rect(206,350, 300, 50),
				String.Format("Fire Breathed: {0:D1} tons", (int)ProgressTracker.FireBreathed),
				textStyle
			);
		});
	}
	
	protected void InCenteredArea(float width, float height, Action drawFn) {
		GUILayout.BeginArea(new Rect( (Screen.width - width) / 2f, (Screen.height - height) / 2f, width, height),
		                    GUI.skin.box);
		
		drawFn();
		
		GUILayout.EndArea();
	}
}