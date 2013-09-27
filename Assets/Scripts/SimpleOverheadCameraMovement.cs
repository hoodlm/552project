using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement script for an overhead camera, using WASD controls to look around the map.
/// </summary>
public class SimpleOverheadCameraMovement : MonoBehaviour {
	
	public float maxAcceleration = 4f;
	public float maxMoveSpeed = 4f;
	public float dragCoefficient = 0.4f;
	
	private Vector3 currentVelocity = Vector3.zero;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 acceleration = AccelerationFromUserInput();
		Vector3 drag = CalculateDrag();
		
		if (acceleration.Equals(Vector3.zero)) {
			currentVelocity += drag * Time.deltaTime;
		} else if (currentVelocity.sqrMagnitude <= maxMoveSpeed * maxMoveSpeed) {
			currentVelocity += acceleration * Time.deltaTime;
		}
		
		MoveCamera(currentVelocity * Time.deltaTime);
	}
	
	private Vector3 AccelerationFromUserInput() {
		Vector3 localAcceleration = Vector3.zero;
		localAcceleration += Input.GetAxis("Horizontal") * Vector3.right;
		localAcceleration += Input.GetAxis("Vertical") * Vector3.forward;
		
		localAcceleration = localAcceleration.normalized * maxAcceleration;
		
		return localAcceleration;
	}
	
	private Vector3 CalculateDrag() {
		Vector3 drag = -currentVelocity;
		return drag * dragCoefficient;
	}
	
	private void MoveCamera(Vector3 trajectory) {
		transform.Translate(trajectory, Space.World);
	}
}
