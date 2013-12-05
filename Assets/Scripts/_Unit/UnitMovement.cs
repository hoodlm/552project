using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A Unit is any soldier, monster, or other agent that moves around the battlefield.
/// Units do not move autonomously - they must be given a target point by a controller (e.g. the player or an AI).
/// WARNING: Make sure that Units have a capsule collider, not a mesh collider or cube collider.
/// </summary>
public class UnitMovement : MonoBehaviour {
	
	private GameObject playerController;
	
	/// <summary>
	/// The speed at which this unit will walk to a target point.
	/// </summary>
	public float walkingSpeed = 10f;
	
	/// <summary>
	/// This unit's stats, primarily used to calculate the range that this unit can move in a single turn.
	/// </summary>
	private UnitInfo unitInfo;
	
	/// <summary>
	/// The original location of this unit at the time that it receives a move order.
	/// </summary>
	private Vector3 origin;
	
	/// <summary>
	/// Keep track of the last known position this unit's transform has had.
	/// </summary>
	private Vector3 previousPosition;
	
	/// <summary>
	/// The number of frames in a row this unit has been determined to be stuck.
	/// </summary>
	private int stuckCounter;
	
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
		playerController = GameObject.FindGameObjectWithTag("Player");
		
		animation.Play("Idle");
		animation["Idle"].speed = 0.70f;
		if (Random.Range(0,2) == 0) {
			animation["Idle2"].speed = -0.60f;
			animation.Play("Idle2");
		}
		hasTarget = false;
		stuckCounter = 0;
		origin = transform.position;
		unitInfo = this.GetComponent<UnitInfo>();
		previousPosition = transform.position;
		rigidbody.constraints = 
			RigidbodyConstraints.FreezePosition |
			RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(Vector3.down * Time.deltaTime * 100f);
		if (hasTarget) {
			MoveTowardsWaypoint(target);
			if (UnitIsStuck()) {
				Debug.Log(this.name + " appears to be stuck.");
				stuckCounter = 0;
				hasTarget = false;
				rigidbody.constraints = 
					RigidbodyConstraints.FreezePosition |
					RigidbodyConstraints.FreezeRotation;
				animation.Play("Idle");
				BroadcastMessage("DoneMoving", StuckResponse(), SendMessageOptions.RequireReceiver);
			}
			if (HasReachedWaypoint(target)) {
				Debug.Log(this.name + " has reached its move target.");
				stuckCounter = 0;
				hasTarget = false;
				rigidbody.constraints = 
					RigidbodyConstraints.FreezePosition |
					RigidbodyConstraints.FreezeRotation;
				animation.Play("Idle");
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
				string debugString = string.Format
					("Move was of distance {0}, but unit's max distance is {1}",
						(this.transform.position - target).magnitude,
						unitInfo.CalculateWalkingDistance());
				Debug.Log(debugString);
				BroadcastMessage("DoneMoving", OutOfRangeResponse(), SendMessageOptions.RequireReceiver);
				playerController.SendMessage("ChangeGUIState", "StartTurn");
			} else {
				animation.Play("Walk");
				this.hasTarget = true;
				//transform.LookAt(target);
				transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
				rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}
	}
	
	/// <summary>
	/// Is the test position within the movement range of this unit?
	/// </summary>
	private bool WithinRange(Vector3 testPosition) {
		float walkingRange = unitInfo.CalculateWalkingDistance();
		
		// We compare based on squared distance for performance reasons
		float distanceSquared = (this.transform.position - testPosition).sqrMagnitude;
		return (distanceSquared <= walkingRange * walkingRange);
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
	/// Determines whether the unit is stuck.
	/// </summary>
	private bool UnitIsStuck() {
		if (Vector3.Distance(transform.position, previousPosition) <= 0.90f * walkingSpeed * Time.deltaTime) {
			stuckCounter++;
		}
		
		previousPosition = transform.position;
		return (stuckCounter >= 30);
	}
	
	/// <summary>
	/// Factory method to build a move response for an out of range position.
	/// </summary>
	private UnitMoveResponse OutOfRangeResponse() {
		return new UnitMoveResponse(this.moveRequest.caller, this.transform.position, 0f, false, "Out of range");
	}
	
	/// <summary>
	/// Factory method to build a move response for a successfully executed move.
	/// </summary>
	private UnitMoveResponse SuccessfulResponse() {
		float distance = (this.transform.position - this.origin).magnitude;
		return new UnitMoveResponse(this.moveRequest.caller, this.transform.position, distance);
	}
	
	/// <summary>
	/// Factory method to build a move response for a unit that got stuck trying to complete a move.
	/// </summary>
	private UnitMoveResponse StuckResponse() {
		float distance = (this.transform.position - this.origin).magnitude;
		return new UnitMoveResponse(this.moveRequest.caller, this.transform.position, distance, true, "Unit got stuck");
	}
}
