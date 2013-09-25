using UnityEngine;
using System.Collections;

/// <summary>
/// Tests the UnitMovement script by sending a test unit to the camera's location if the user presses SPACE.
/// </summary>
public class UnitMovementTestController : MonoBehaviour {
	
	public GameObject TestUnit;
	
	private Vector3 Target;
	private GameObject Ground;
	
	// Use this for initialization
	void Start () {
		Ground = GameObject.FindWithTag("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			TestUnitMovement();
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(0,0,200,100), "Left click to move the \n cube to the target position.");
	}
	
	public void UnitFinishedMovement () {
		float distance = Vector3.Distance(Target, TestUnit.transform.position);
		string debugString = string.Format("Unit's distance from target point: {0}", distance);
		print (debugString);
	}
	
	private void TestUnitMovement () {
		Target = GetPositionOnTerrain();
			
		string debugString = string.Format("Target position: {0},{1},{2}", Target.x, Target.y, Target.z);
		print(debugString);
		
		TestUnit.GetComponent<UnitMovement>().GiveWalkingTarget(Target, this.gameObject);
	}
	
	private Vector3 GetPositionOnTerrain() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		Ground.collider.Raycast(ray, out hit, float.PositiveInfinity);
		return hit.point;
	}
}
