using UnityEngine;
using System.Collections;

public class PlayerController : AbstractController {
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Moves the unit that this controller is currently controlling.
	/// </summary>
	override protected void SendMoveOrderToUnit(Vector3 target) {
			
		string debugString = 
			string.Format("Target move position: {0},{1},{2}", target.x, target.y, target.z);
		Debug.Log(debugString);
		
		UnitMoveRequest request = new UnitMoveRequest(this.gameObject, target);
		
		Debug.Log("Sending move order to " + currentUnit.name);
		currentUnit.SendMessage("RequestMove", request, SendMessageOptions.RequireReceiver);
	}
	
	/// <summary>
	/// Notify this controller than the current unit has finished its current move order.
	/// </summary>
	override protected void UnitDoneMoving(UnitMoveResponse response) {
		if (!response.validMove) {
			string debugString = 
				string.Format ("{0} received a report that {1}'s move was invalid (\"{2}\").",
					this.name, currentUnit.name, response.responseMessage);
			Debug.Log(debugString);
			
			// We allow another move request if this one was invalid.
			// TODO: notify the user that the move was illegal.
			
		} else {
			hasAlreadyMoved = true;
			string debugString = 
				string.Format ("{0} received a report that {1} finished moving.", this.name, currentUnit.name);
			Debug.Log(debugString);
		}
	}
}
