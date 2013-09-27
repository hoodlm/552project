using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple prototype turn scheduler, which iterates over a list of Units in a fixed order.
/// </summary>
public class SimpleTurnScheduler : MonoBehaviour {
	
	public List<GameObject> units;
	
	private int turnCounter;
	
	// Use this for initialization
	void Start () {
		// Initialize TurnCounter at -1 so that Unit 0 goes first.
		turnCounter = -1;
		NextTurn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Whose turn is it anyway?
	/// </summary>
	public GameObject WhoseTurn() {
		return units[turnCounter];
	}
	
	/// <summary>
	/// Ends the current Unit's turn and notifies the next Unit that it is his turn.
	/// </summary>
	public virtual void NextTurn() {
		if (turnCounter != -1) {
			WhoseTurn().SendMessage("FinishTurn", SendMessageOptions.RequireReceiver);
		}
		Debug.Log(this.name + " is advancing the TurnCounter from " + turnCounter);
		turnCounter = (turnCounter + 1) % units.Count;
		units[turnCounter].SendMessage("BeginTurn", this.gameObject);
	}
}
