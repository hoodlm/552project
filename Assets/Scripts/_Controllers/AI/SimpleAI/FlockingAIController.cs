using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockingAIController : AbstractController {
	
	/// <summary>
	/// To create the illusion of complex AI
	/// </summary>
	public float thinkingTime;
	
	private float thinkingTimer;
	
	/// <summary>
	/// A list of all active units controlled by the opponent.
	/// </summary>
	private IList<GameObject> opponentUnits;
	
	/// <summary>
	/// Does this unit have a flocking target?
	/// </summary>
	private bool hasTarget;
	
	/// <summary>
	/// The current target for flocking.
	/// </summary>
	private GameObject currentTarget;
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = GameObject.FindGameObjectWithTag("Player");
		opponentUnits = FindOpponentUnits();
		hasTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
		thinkingTimer += Time.deltaTime;
		if (inAction && !hasTarget) {
			currentTarget = GetClosestUnitFromList(opponentUnits);
			Debug.Log(currentUnit.name + " is now targeting " + currentTarget.name);
			hasTarget = true;
			thinkingTimer = 0f;
			currentUnit.SendMessage("ShowMovementRadius", SendMessageOptions.RequireReceiver);
		} else if (inAction && !hasAlreadyMoved && !isMoving && hasTarget && (thinkingTimer >= thinkingTime)) {
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
		} else if (inAction && hasAlreadyMoved) {
			hasTarget = false;
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
	
	/// <summary>
	/// Instruct the unit to move to a set point.
	/// </summary>
	override protected void SendMoveOrderToUnit(Vector3 target) {
		string debugString = 
			string.Format("Target move position: {0},{1},{2}", target.x, target.y, target.z);
		Debug.Log(debugString);
		
		isMoving = true;
		
		UnitMoveRequest request = new UnitMoveRequest(this.gameObject, target);
		
		Debug.Log("Sending move order to " + currentUnit.name);
		currentUnit.SendMessage("RequestMove", request, SendMessageOptions.RequireReceiver);
	}
	
	/// <summary>
	/// Since the AI never moves the unit, this method body is empty.
	/// </summary>
	override protected void UnitDoneMoving(UnitMoveResponse response) {
		if (!response.validMove) {
			string debugString = 
				string.Format ("{0} received a report that {1}'s move was invalid (\"{2}\").",
					this.name, currentUnit.name, response.responseMessage);
			Debug.Log(debugString);
			
			// TODO: Allow the AI to handle this somehow, rather than giving up.
			hasAlreadyMoved = true;
			isMoving = false;
			
		} else {
			string debugString = 
				string.Format ("{0} received a report that {1} finished moving.", this.name, currentUnit.name);
			Debug.Log(debugString);
			
			hasAlreadyMoved = true;
			isMoving = false;
		}
	}
}
