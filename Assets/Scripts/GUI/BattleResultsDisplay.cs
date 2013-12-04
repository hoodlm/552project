using UnityEngine;
using System.Collections;

public class BattleResultsDisplay : MonoBehaviour {
	
	private bool displaying;
	private BattleResult outcome;
	
	// Use this for initialization
	void Start () {
		displaying = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DisplayBattleResults(BattleResult outcome) {
		this.outcome = outcome;
		this.displaying = true;
	}
	
	void OnGUI() {
		if (displaying) {
			
			string s = string.Format
				("BATTLE OVER\n" +
					"The winner was {0}\n" +
					"with {1} units left",
					outcome.victor,
					outcome.unitsLeft);
			
			Rect displayRect = new Rect(0f, 0f, Screen.width, Screen.height);
			GUI.Box(displayRect, s);
			
			Rect buttonRect = new Rect
				(Screen.width / 4, Screen.height / 4, Screen.width / 2, 100);
			if (GUI.Button(buttonRect, "Return to Main Menu")) {
				Application.LoadLevel(0);
			}
		}
	}
}
