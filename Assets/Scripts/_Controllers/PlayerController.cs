using UnityEngine;
using System.Collections;

public class PlayerController : AbstractController {
	
	// Use this for initialization
	void Start () {
		ground = GameObject.FindWithTag("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			SendMoveOrderToUnit();
		}
	}
	
	override protected void SendMoveOrderToUnit() {
		Vector3 target = GetCursorPositionOnTerrain();
			
		string debugString = 
			string.Format("Target move position: {0},{1},{2}", target.x, target.y, target.z);
		Debug.Log(debugString);
		
		Debug.Log("Sending move order to " + currentUnit.name);
		currentUnit.GetComponent<UnitMovement>().GiveWalkingTarget(target, gameObject);
	}
	
	override protected void UnitFinishedMoveOrder(float distanceMoved) {
		string debugString = 
			string.Format ("{0} received a report that {1} finished moving.", this.name, currentUnit.name);
		Debug.Log(debugString);
		currentUnit.SendMessage("FinishTurn", SendMessageOptions.RequireReceiver);
	}
	
	private Vector3 GetCursorPositionOnTerrain() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		ground.collider.Raycast(ray, out hit, float.PositiveInfinity);
		return hit.point;
	}
}
