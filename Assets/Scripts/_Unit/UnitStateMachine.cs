using UnityEngine;
using System.Collections;

/// <summary>
/// Models the possible states that a unit is in.
/// </summary>
public class UnitStateMachine : MonoBehaviour {
	
	private enum State {WaitingTurn, WaitingOrders, Moving, Attacking};
	
	private State currentState = State.WaitingTurn;
	
	private GameObject playerController;
	
	// Use this for initialization
	void Start () {
		playerController = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Notify the unit to begin its turn.
	/// </summary>
	public void BeginTurn (GameObject caller) {
		if (currentState == State.WaitingTurn) {
			currentState = State.WaitingOrders;
			string LogMsg = "{0} is starting its turn (called by {1})";
			Debug.Log (string.Format(LogMsg, this.name, caller.name));
			
		} else {
			string LogMsg = "BeginTurn unexpectedly called on {0} while it is in state \"{1}\" (called by {2})";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString(), caller.name));
		}
	}
	
	/// <summary>
	/// Notify the unit to end its turn.
	/// </summary>
	public void FinishTurn (GameObject caller) {
		if (currentState == State.WaitingOrders) {
			currentState = State.WaitingTurn;
			string LogMsg = "{0} is ending its turn (called by {1})";
			Debug.Log (string.Format(LogMsg, this.name, caller.name));
		} else {
			string LogMsg = "FinishTurn unexpectedly called on {0} while it is in state \"{1}\" (called by {2})";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString(), caller.name));
		}
	}
	
	/// <summary>
	/// Requests that the unit move to a certain location.
	/// </summary>
	public void RequestMove (UnitMoveRequest request) {
		if (currentState == State.WaitingOrders) {
			
			currentState = State.Moving;
			BroadcastMessage("Move", request, SendMessageOptions.RequireReceiver);
			playerController.SendMessage("ChangeGUIState", "WaitingForEnemyTurn");
		} else if (currentState == State.Moving) {
			string LogMsg = "RequestMove called on {0}, but it's already moving (called by {1})";
			Debug.Log(string.Format(LogMsg, this.name, request.caller.name));
		} else {
			string LogMsg = "RequestMove unexpectedly called on {0} while it is in state \"{1}\" (called by {2})";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString(), request.caller.name));
		}
	}
	
	/// <summary>
	/// Called when the unit has finished moving.
	/// </summary>
	public void DoneMoving (UnitMoveResponse response) {
		if (currentState == State.Moving) {
			string LogMsg = "{0} is done moving.";
			Debug.Log (string.Format(LogMsg, this.name));
			currentState = State.WaitingOrders;
			response.caller.SendMessage("UnitDoneMoving", response, SendMessageOptions.RequireReceiver);
			playerController.SendMessage("ChangeGUIState", "StartTurn");
			BroadcastMessage("HideMovementRadius", SendMessageOptions.RequireReceiver);
		} else {
			string LogMsg = "RequestMove unexpectedly called on {0} while it is in state \"{1}\"";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString()));
		}
	}
	
	/// <summary>
	/// Requests that the unit attack another unit.
	/// </summary>
	public void RequestAttack (UnitAttackRequest request) {
		if (currentState == State.WaitingOrders) {
			
			currentState = State.Attacking;
			SendMessage("ExecuteAttack", request, SendMessageOptions.RequireReceiver);
			
		} else if (currentState == State.Attacking) {
			string LogMsg = "RequestAttack called on {0}, but it's already moving (called by {1})";
			Debug.Log(string.Format(LogMsg, this.name, request.attacker.name));
		} else {
			string LogMsg = "RequestMove unexpectedly called on {0} while it is in state \"{1}\" (called by {2})";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString(), request.attacker.name));
		}
	}
	
	/// <summary>
	/// Called when the unit has finished its attack.
	/// </summary>
	public void DoneAttacking (UnitAttackResponse response) {
		if (currentState == State.Attacking) {
			string LogMsg = "{0} is done attacking.";
			Debug.Log (string.Format(LogMsg, this.name));
			currentState = State.WaitingOrders;
			response.caller.SendMessage("UnitDoneAttacking", response, SendMessageOptions.RequireReceiver);
			playerController.SendMessage("ChangeGUIState", "StartTurn");
			BroadcastMessage("HideAttackRadius", SendMessageOptions.RequireReceiver);
		} else {
			string LogMsg = "DoneAttacking unexpectedly called on {0} while it is in state \"{1}\"";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString()));
		}
	}
}
