using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement script for an overhead camera, using WASD controls to look around the map.
/// </summary>
public class SimpleOverheadCameraMovement : MonoBehaviour {
	
	public float MoveSpeed = 20f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 trajectory = GetTrajectoryFromUserInput();
		MoveCamera(trajectory);
	}
	
	private Vector3 GetTrajectoryFromUserInput() {
		Vector3 trajectory = Vector3.zero;
		trajectory += Input.GetAxis("Horizontal") * Vector3.right;
		trajectory += Input.GetAxis("Vertical") * Vector3.forward;
		
		trajectory = trajectory.normalized * MoveSpeed * Time.deltaTime;
		
		return trajectory;
	}
	
	private void MoveCamera(Vector3 trajectory) {
		transform.Translate(trajectory, Space.World);
	}
}
