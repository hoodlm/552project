using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper object with the information needed for an attack response from a unit.
/// </summary>
public class UnitAttackResponse {

	/// <summary>
	/// The GameObject (controller) that originally sent this request.
	/// </summary>
	public readonly GameObject caller;
	
	/// <summary>
	/// The damage performed by the attack.
	/// </summary>
	public readonly float damage;
	
	/// <summary>
	/// Was the attack evaded/dodged?
	/// </summary>
	public readonly bool wasEvaded;
	
	/// <summary>
	/// Was the attack valid? (i.e. did the unit even attempt to attack?)
	/// </summary>
	public readonly bool validAttack;
	
	/// <summary>
	/// For debugging purposes only, a message describing why the attack was invalid.
	/// Do not use this for any comparators! If we need more error response functionality
	/// we can add extra flags to the response.
	/// </summary>
	public readonly string responseMessage;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitAttackResponse"/> class for a valid move.
	/// </summary>
	public UnitAttackResponse(GameObject caller, bool wasEvaded, float damage) {
		this.caller = caller;
		this.wasEvaded = wasEvaded;
		this.damage = damage;
		this.validAttack = true;
		this.responseMessage = string.Empty;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitAttackResponse"/> class.
	/// </summary>
	public UnitAttackResponse(GameObject caller, bool wasEvaded, float damage, bool validAttack, string responseMessage) {
		this.caller = caller;
		this.wasEvaded = wasEvaded;
		this.damage = damage;
		this.validAttack = validAttack;
		if (string.IsNullOrEmpty(responseMessage)) {
			this.responseMessage = string.Empty;
		} else {
			this.responseMessage = responseMessage;
		}
	}
}