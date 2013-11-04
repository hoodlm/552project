﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockingAIController : AbstractAIController {
	
	/// <summary>
	/// To create the illusion of complex AI
	/// </summary>
	public float thinkingTime;
	private float timer;
	private bool timerRunning = false;
	
	/// <summary>
	/// A list of all active units controlled by the opponent.
	/// </summary>
	private IList<GameObject> opponentUnits;
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = GameObject.FindGameObjectWithTag("Player");
		opponentUnits = FindOpponentUnits();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentPhase == TurnPhase.WaitingTurn && this.IsInAction()) {
			currentPhase = TurnPhase.MovingUnit;
			currentUnit.SendMessage("ShowMovementRadius", SendMessageOptions.RequireReceiver);
			timer = thinkingTime;
			timerRunning = true;
			
		} else if (currentPhase == TurnPhase.MovingUnit
					&& timerRunning) {
			
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				timerRunning = false;
				
				GameObject currentTarget = GetClosestUnitFromList(opponentUnits);
				Debug.Log(currentUnit.name + " is now targeting " + currentTarget.name);
				
				currentUnit.SendMessage("HideMovementRadius", SendMessageOptions.RequireReceiver);
				currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
				
				Vector3 trajectory = currentTarget.transform.position - currentUnit.transform.position;
				float maxDistance = currentUnit.GetComponent<UnitInfo>().CalculateWalkingDistance();
				Vector3 target;
				if (trajectory.sqrMagnitude > maxDistance * maxDistance) {
					target = currentUnit.transform.position + trajectory.normalized * maxDistance * 0.90f;
				} else {
					target = currentUnit.transform.position + trajectory * 0.90f;
				}
				SendMoveOrderToUnit(target);
			}
			
		} else if (currentPhase == TurnPhase.Attacking) {
			currentPhase = TurnPhase.WaitingTurn;
			currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.DontRequireReceiver);
			GiveUpControl();
		}
	}
	
	private IList<GameObject> FindOpponentUnits() {
		List<GameObject> opponentUnits = new List<GameObject>();
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		foreach (GameObject unit in allUnits) {
			if (unit.GetComponent<UnitInfo>().controller.gameObject == player) {
				opponentUnits.Add(unit);
			}
		}
		
		return opponentUnits;
	}
	
	private GameObject GetClosestUnitFromList(IList<GameObject> units) {
		float closestDistance = float.PositiveInfinity;
		GameObject closestObject = null;
		
		foreach(GameObject unit in units) {
			float distance = Vector3.Distance(currentUnit.transform.position, unit.transform.position);
			if (distance <= closestDistance) {
				closestObject = unit;
				closestDistance = distance;
			}
		}
		
		return closestObject;
	}
}
