using UnityEngine;
using System.Collections;

/// <summary>
/// A Unit is any soldier, monster, or other agent that moves around the battlefield.
/// Units do not move autonomously - they must be given a target point by a controller (e.g. the player or an AI).
/// WARNING: Make sure that Units have a capsule collider, not a mesh collider or cube collider.
/// </summary>
public class UnitMovement : MonoBehaviour {
	
	/// <summary>
	/// The speed at which this unit will walk to a target point.
	/// </summary>
	public float walkingSpeed = 10f;
	
	/// <summary>
	/// The range that this unit can move in a single turn.
	/// </summary>
	public float walkingRange = 10f;
	
	/// <summary>
	/// The original location of this unit at the time that it receives a move order.
	/// </summary>
	private Vector3 origin;
	
	/// <summary>
	/// The walking target for this unit.
	/// </summary>
	private Vector3 target;
	private bool hasTarget;
	
	/// <summary>
	/// The most recently received moveRequest.
	/// </summary>
	private UnitMoveRequest moveRequest;
	
	/// <summary>
	/// How close a unit must be to a waypoint to consider it complete.
	/// </summary>
	private static readonly float DISTANCE_THRESHOLD = 0.2f;
	
	// Use this for initialization
	void Start () {
		hasTarget = false;
		origin = transform.position;
		rigidbody.constraints = 
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasTarget) {
			MoveTowardsWaypoint(target);
			if (HasReachedWaypoint(target)) {
				Debug.Log(this.name + " has reached its move target.");
				hasTarget = false;
				rigidbody.constraints = 
					RigidbodyConstraints.FreezePositionX |
					RigidbodyConstraints.FreezePositionZ |
					RigidbodyConstraints.FreezeRotation;
				BroadcastMessage("DoneMoving", SuccessfulResponse(), SendMessageOptions.RequireReceiver);
			}
		}
	}
	
	/// <summary>
	/// Requests that this unit move toward the target point. The unit will ignore incoming requests until
	/// it is done moving.
	/// </summary>
	/// <param name='target'>
	/// The target point, in world coordinates.
	/// </param>
	/// <param name='controller'>
	/// The controller object that is sending the movement request.
	/// </param>
	public void Move(UnitMoveRequest request) {
		if (!hasTarget) {
			this.target = request.targetPosition;
			this.origin = transform.position;
			this.moveRequest = request;
			if (!WithinRange(target)) {
				BroadcastMessage("DoneMoving", OutOfRangeResponse(), SendMessageOptions.RequireReceiver);
			} else {
				this.hasTarget = true;
				transform.LookAt(target);
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}
	}
	
	/// <summary>
	/// Is the test position within the movement range of this unit?
	/// </summary>
	private bool WithinRange(Vector3 testPosition) {
		// We compare based on squared distance for performance reasons
		float distanceSquared = (this.transform.position - testPosition).sqrMagnitude;
		return (distanceSquared <= this.walkingRange * this.walkingRange);
	}
	
	/// <summary>
	/// Moves the unit towards the given waypoint in world coordinates.
	/// </summary>
	private void MoveTowardsWaypoint(Vector3 waypoint) {
		Vector3 trajectory = (waypoint - transform.position).normalized;
		trajectory *= walkingSpeed * Time.deltaTime;
		transform.Translate(trajectory, Space.World);
	}
	
	/// <summary>
	/// Determines whether this unit has reached the specified waypoint in world coordinates.
	/// </summary>
	private bool HasReachedWaypoint(Vector3 waypoint) {
		float distance = Vector3.Distance(waypoint, transform.position);
		return (distance < DISTANCE_THRESHOLD);
	}
	
	/// <summary>
	/// Factory method to build a move response for an out of range position.
	/// </summary>
	/// <param name='request'>
	/// The original move request object.
	/// </param>
	private UnitMoveResponse OutOfRangeResponse() {
		return new UnitMoveResponse(this.moveRequest.caller, this.transform.position, 0f, false, "Out of range");
	}
	
	/// <summary>
	/// Factory method to build a move response for a successfully executed move.
	/// </summary>
	/// <param name='request'>
	/// The original move request object.
	/// </param>
	private UnitMoveResponse SuccessfulResponse() {
		float distance = (this.transform.position - this.origin).magnitude;
		return new UnitMoveResponse(this.moveRequest.caller, this.transform.position, distance);
	}
}
