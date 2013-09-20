using UnityEngine;
using System.Collections;

/// <summary>
/// Tests the UnitMovement script by sending a test unit to the camera's location if the user presses SPACE.
/// </summary>
public class UnitMovementTest : MonoBehaviour {
	
	public GameObject TestUnit;
	
	private Vector3 Target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			TestUnitMovement();
		}
	}
	
	public void UnitFinishedMovement () {
		float distance = Vector3.Distance(Target, TestUnit.transform.position);
		string debugString = string.Format("Unit's distance from target point: {0}", distance);
		print (debugString);
	}
	
	private void TestUnitMovement () {
		// Our target position will be directly below the camera, so eliminate y component.
		Target = transform.position;
		Target.y = 0f;
			
		string debugString = string.Format("Target position: {0},{1},{2}", Target.x, Target.y, Target.z);
		print(debugString);
		
		TestUnit.GetComponent<UnitMovement>().GiveWalkingTarget(Target, this.gameObject);
	}
}
