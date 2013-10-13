using UnityEngine;
using System.Collections;

public class PlayerController : AbstractController {
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			SendMoveOrderToUnit();
		}
	}
	
	/// <summary>
	/// Moves the unit that this controller is currently controlling.
	/// </summary>
	override protected void SendMoveOrderToUnit() {
		Vector3 target = GetCursorPositionOnTerrain();
			
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
		string debugString = 
			string.Format ("{0} received a report that {1} finished moving.", this.name, currentUnit.name);
		Debug.Log(debugString);
		this.GiveUpControl();
		scheduler.SendMessage("NextTurn", SendMessageOptions.RequireReceiver);
	}
	
	/// <summary>
	/// Helper method to retrieve the cursor position on terrain.
	/// </summary>
	/// <returns>
	private Vector3 GetCursorPositionOnTerrain() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		ground.collider.Raycast(ray, out hit, float.PositiveInfinity);
		return hit.point;
	}
}
