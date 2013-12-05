using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement script for an overhead camera, using WASD controls to look around the map.
/// </summary>
public class SimpleOverheadCameraMovement : MonoBehaviour {
	
	public float maxAcceleration = 4f;
	public float maxMoveSpeed = 4f;
	public float dragCoefficient = 0.4f;
	public float zoomSpeed = 5f;
	public float minHeight = 5f;
	public float maxHeight = 50f;
	
	private Vector3 currentVelocity = Vector3.zero;
	
	public float autoMoveSpeed = 12f;
	private bool cameraIsAutoMoving = false;
	private Vector3 autoMoveTarget = Vector3.zero;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (cameraIsAutoMoving) {
			MoveCameraTowardTarget();
		} else {
			ZoomingUserInput();
			SimpleMovementUserInput();
		}
	}
	
	/// <summary>
	/// Can be called by external classes for camera movement "scripts".
	/// Locks down all camera controls and moves the camera to the target position (ignoring the y coordinate).
	/// </summary>
	public void AutoMoveCameraTo(Vector3 target) {
		cameraIsAutoMoving = true;
		autoMoveTarget = new Vector3(target.x, this.transform.position.y, target.z - 3f);
	}
	
	private void MoveCameraTowardTarget() {
		Vector3 trajectory = autoMoveTarget - this.transform.position;
		
		if (trajectory.sqrMagnitude <= autoMoveSpeed * autoMoveSpeed) {
			cameraIsAutoMoving = false;
		} else {
			MoveCamera(trajectory.normalized * autoMoveSpeed * Time.deltaTime * (transform.position.y + 50));
		}
	}
	
	private void ZoomingUserInput() {
		float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		if ((zoom > 0f && transform.position.y >= minHeight) ||
			(zoom < 0f && transform.position.y <= maxHeight)) {
			
			transform.position += transform.forward * zoom * Time.deltaTime;
		}
	}
	
	private void SimpleMovementUserInput() {
		Vector3 acceleration = AccelerationFromUserInput();
		Vector3 drag = CalculateDrag();
		
		currentVelocity += acceleration * Time.deltaTime;
		float maxVelSqr = maxMoveSpeed * maxMoveSpeed * transform.position.y * (transform.position.y + 50);
		
		if (acceleration.Equals(Vector3.zero)) {
			currentVelocity += drag * Time.deltaTime;
		} else if (currentVelocity.sqrMagnitude >= maxVelSqr) {
			currentVelocity = currentVelocity.normalized * maxMoveSpeed * (transform.position.y + 50);
		}
		
		MoveCamera(currentVelocity * Time.deltaTime);
	}
	
	private Vector3 AccelerationFromUserInput() {
		Vector3 localAcceleration = Vector3.zero;
		localAcceleration += Input.GetAxis("Horizontal") * Vector3.right;
		localAcceleration += Input.GetAxis("Vertical") * Vector3.forward;
		
		localAcceleration = localAcceleration.normalized * maxAcceleration;
		
		// Also scale acceleration with zoom height
		localAcceleration *= (transform.position.y + 50);
		
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
