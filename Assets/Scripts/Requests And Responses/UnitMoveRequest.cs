using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper object with the information needed for a move request sent to a unit.
/// </summary>
public class UnitMoveRequest {
	
	/// <summary>
	/// The GameObject sending this request.
	/// </summary>
	public readonly GameObject caller;
	
	/// <summary>
	/// The position for the unit to attempt to move to.
	/// </summary>
	public readonly Vector3 targetPosition;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitMoveRequest"/> class.
	/// </summary>
	/// <param name='caller'>
	/// The GameObject sending this request.
	/// </param>
	/// <param name='targetPosition'>
	/// The position for the unit to attempt to move to.
	/// </param>
	public UnitMoveRequest(GameObject caller, Vector3 targetPosition) {
		this.caller = caller;
		this.targetPosition = targetPosition;
	}
	
}
