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
	public float WalkingSpeed = 10f;
	
	/// <summary>
	/// The walking target for this unit.
	/// </summary>
	private Vector3 Target;
	private bool HasTarget;
	
	/// <summary>
	/// How close a unit must be to a waypoint to consider it complete.
	/// </summary>
	private static float DistanceThreshold = 0.2f;
	
	/// <summary>
	/// The controller that sent the most recent move order.
	/// </summary>
	private GameObject MoveController;
	
	// Use this for initialization
	void Start () {
		HasTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (HasTarget) {
			MoveTowardsWaypoint(Target);
			if (HasReachedWaypoint(Target)) {
				MoveController.SendMessage("UnitFinishedMovement");
				HasTarget = false;
				rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
			}
		}
	}
	
	/// <summary>
	/// Requests that this unit move toward the target point. The unit will get as close to this point as possible.
	/// </summary>
	/// <param name='target'>
	/// The target point, in world coordinates.
	/// </param>
	/// <param name='controller'>
	/// The controller object that is sending the movement request.
	/// </param>
	public void GiveWalkingTarget(Vector3 target, GameObject controller) {
		this.Target = target;
		this.HasTarget = true;
		this.MoveController = controller;
		transform.LookAt(target);
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	/// <summary>
	/// Moves the unit towards the given waypoint in world coordinates.
	/// </summary>
	private void MoveTowardsWaypoint(Vector3 waypoint) {
		Vector3 trajectory = (waypoint - transform.position).normalized;
		trajectory *= WalkingSpeed * Time.deltaTime;
		transform.Translate(trajectory, Space.World);
	}
	
	/// <summary>
	/// Determines whether this unit has reached the specified waypoint in world coordinates.
	/// </summary>
	private bool HasReachedWaypoint(Vector3 waypoint) {
		float distance = Vector3.Distance(waypoint, transform.position);
		return (distance < DistanceThreshold);
	}
}
