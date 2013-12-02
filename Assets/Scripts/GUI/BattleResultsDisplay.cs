using UnityEngine;
using System.Collections;

public class BattleResultsDisplay : MonoBehaviour {
	
	private bool displaying;
	
	// Use this for initialization
	void Start () {
		displaying = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DisplayBattleResults() {
		displaying = true;
	}
	
	void OnGUI() {
		if (displaying) {
			Rect displayRect = new Rect(0f, 0f, Screen.width, Screen.height);
			GUI.Box(displayRect, "BATTLE OVER!");
		}
	}
}
