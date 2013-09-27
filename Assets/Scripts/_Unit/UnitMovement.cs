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
	/// The original location of this unit at the time that it receives a move order.
	/// </summary>
	private Vector3 origin;
	
	/// <summary>
	/// The walking target for this unit.
	/// </summary>
	private Vector3 target;
	private bool hasTarget;
	
	/// <summary>
	/// How close a unit must be to a waypoint to consider it complete.
	/// </summary>
	private static readonly float DISTANCE_THRESHOLD = 0.2f;
	
	/// <summary>
	/// The controller that sent the most recent move order.
	/// </summary>
	private GameObject controller;
	
	// Use this for initialization
	void Start () {
		hasTarget = false;
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasTarget) {
			MoveTowardsWaypoint(target);
			if (HasReachedWaypoint(target)) {
				float distanceTravelled = Vector3.Distance(origin, transform.position);
				Debug.Log(this.name + " has reached its move target.");
				controller.SendMessage("UnitFinishedMoveOrder", distanceTravelled);
				hasTarget = false;
				rigidbody.constraints = 
					RigidbodyConstraints.FreezePosition |
					RigidbodyConstraints.FreezeRotation;
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
	public void GiveWalkingTarget(Vector3 target, GameObject controller) {
		if (!hasTarget) {
			this.target = target;
			this.hasTarget = true;
			this.controller = controller;
			this.origin = transform.position;
			transform.LookAt(target);
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
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
}
