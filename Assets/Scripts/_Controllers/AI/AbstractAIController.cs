using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract class for AI implementations to utilize.
/// </summary>
public abstract class AbstractAIController : AbstractController {

	protected enum TurnPhase {WaitingTurn, MovingUnit, Attacking};
	protected TurnPhase currentPhase = TurnPhase.WaitingTurn;
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPhase == TurnPhase.WaitingTurn && this.IsInAction()) {
			currentPhase = TurnPhase.MovingUnit;
		}
	}
	
	/// <summary>
	/// Moves the unit that this controller is currently controlling.
	/// </summary>
	override protected void SendMoveOrderToUnit(Vector3 target) {
		if (currentPhase != TurnPhase.MovingUnit) {
			Debug.LogWarning(
			string.Format("SendMoveOrderToUnit unexpectedly called while {0} was in state {1}",
							this.name, currentPhase.ToString()));
		} else {
					
			string debugString = 
				string.Format("Target move position: {0},{1},{2}", target.x, target.y, target.z);
			Debug.Log(debugString);
			
			UnitMoveRequest request = new UnitMoveRequest(this.gameObject, target);
			
			Debug.Log("Sending move order to " + currentUnit.name);
			currentUnit.SendMessage("RequestMove", request, SendMessageOptions.RequireReceiver);
		}
	}
	
	/// <summary>
	/// Notify this controller than the current unit has finished its current move order.
	/// </summary>
	override protected void UnitDoneMoving(UnitMoveResponse response) {
		if (currentPhase != TurnPhase.MovingUnit) {
			Debug.LogWarning(
				string.Format("UnitDoneMoving unexpectedly called while {0} was in state {1}",
							this.name, currentPhase.ToString()));
			
		} else if (!response.validMove) {
			string debugString = 
				string.Format ("{0} received a report that {1}'s move was invalid (\"{2}\").",
					this.name, currentUnit.name, response.responseMessage);
			Debug.Log(debugString);
			
			// We don't allow another move request if this one was invalid!
			currentPhase = TurnPhase.Attacking;
			// TODO: Allow the AI to retry?
			
		} else {
			string debugString = 
				string.Format ("{0} received a report that {1} finished moving.", this.name, currentUnit.name);
			Debug.Log(debugString);
			
			currentPhase = TurnPhase.Attacking;
		}
	}
}
