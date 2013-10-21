using UnityEngine;
using System.Collections;

/// <summary>
/// Models the possible states that a unit is in.
/// </summary>
public class UnitStateMachine : MonoBehaviour {
	
	private enum State {WaitingTurn, WaitingOrders, Moving, UsingAbility};
	
	private State currentState = State.WaitingTurn;
	
	// Use this for initialization
	void Start () {
		
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
			BroadcastMessage("Move", request);
			
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
			
			BroadcastMessage("HideMovementRadius", SendMessageOptions.RequireReceiver);
		} else {
			string LogMsg = "RequestMove unexpectedly called on {0} while it is in state \"{1}\"";
			Debug.LogWarning(string.Format(LogMsg, this.name, currentState.ToString()));
		}

	}
}
