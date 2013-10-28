﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple prototype turn scheduler, which iterates over a list of Units in a fixed order.
/// </summary>
public class SimpleTurnScheduler : MonoBehaviour {
	
	/// <summary>
	/// The ordered list of units
	/// </summary>
	public List<GameObject> units;
	
	/// <summary>
	/// A mapping from Unit to Controller
	/// </summary>
	private Dictionary<GameObject, AbstractController> controllers;
	
	private int turnCounter;
	
	/// <summary>
	/// We wait a few seconds before we start, to make sure all initialization is done.
	/// </summary>
	private float startTimer = 3f;
	
	// Use this for initialization
	void Start () {
		RebuildControllerMapping();
		// Initialize TurnCounter at -1 so that Unit 0 goes first.
		turnCounter = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (turnCounter < 0 && Time.timeSinceLevelLoad > startTimer) {
			NextTurn();
		}
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
	virtual public void NextTurn() {
		Debug.Log(this.name + " is advancing the TurnCounter from " + turnCounter);
		turnCounter = (turnCounter + 1) % units.Count;
		GameObject unit = units[turnCounter];
		controllers[unit].TakeControlOf(unit);
	}
	
	/// <summary>
	/// Builds the mapping of units to controllers.
	/// </summary>
	protected void RebuildControllerMapping() {
		controllers = new Dictionary<GameObject, AbstractController>();
		foreach (GameObject unit in units) {
			AbstractController controller = unit.GetComponent<UnitInfo>().controller;
			controllers.Add(unit, controller);
		}
	}
}
