using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple prototype turn scheduler, which iterates over a list of Units in a fixed order.
/// </summary>
public class SimpleTurnScheduler : MonoBehaviour {
	
	public List<GameObject> Units;
	
	private int TurnCounter;
	
	// Use this for initialization
	void Start () {
		// Initialize TurnCounter at -1 so that Unit 0 goes first.
		TurnCounter = -1;
		NextTurn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Whose turn is it anyway?
	/// </summary>
	public GameObject WhoseTurn() {
		return Units[TurnCounter];
	}
	
	/// <summary>
	/// Ends the current Unit's turn and notifies the next Unit that it is his turn.
	/// </summary>
	public void NextTurn() {
		Debug.Log(this.name + " is advancing the TurnCounter from " + TurnCounter);
		TurnCounter = (TurnCounter + 1) % Units.Count;
		Units[TurnCounter].GetComponent<UnitTurnTest>().StartTurn(gameObject);
	}
}
