using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper class with the response from a unit, after receiving (and hopefully executing) a move request.
/// </summary>
public class UnitMoveResponse {

	/// <summary>
	/// The caller that originally sent the move request.
	/// </summary>
	public readonly GameObject caller;
	
	/// <summary>
	/// The position that the unit ended up in after attempting to move.
	/// </summary>
	public readonly Vector3 resultPosition;
	
	/// <summary>
	/// The distance travelled during this move.
	/// </summary>
	public readonly float distanceTravelled;
	
	/// <summary>
	/// Was the move valid? (i.e. did the unit even attempt to move?)
	/// </summary>
	public readonly bool validMove;
	
	/// <summary>
	/// For debugging purposes only, a message describing why the move was invalid.
	/// Do not use this for any comparators! If we need more error response functionality
	/// we can add extra flags to the response.
	/// </summary>
	public readonly string responseMessage;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitMoveResponse"/> class, assuming a valid move.
	/// </summary>
	public UnitMoveResponse (GameObject caller, Vector3 resultPosition, float distanceTravelled) {
		this.caller = caller;
		this.resultPosition = resultPosition;
		this.distanceTravelled = distanceTravelled;
		this.validMove = true;
		this.responseMessage = string.Empty;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitMoveResponse"/> class.
	/// </summary>
	public UnitMoveResponse (GameObject caller, Vector3 resultPosition, float distanceTravelled, bool validMove, string responseMessage) {
		this.caller = caller;
		this.resultPosition = resultPosition;
		this.distanceTravelled = distanceTravelled;
		this.validMove = validMove;
		if (string.IsNullOrEmpty(responseMessage)) {
			responseMessage = string.Empty;
		} else {
			this.responseMessage = responseMessage;
		}
	}
	
}
