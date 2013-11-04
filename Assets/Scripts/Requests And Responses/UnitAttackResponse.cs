using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper object with the information needed for an attack response from a unit.
/// </summary>
public class UnitAttackResponse {

	/// <summary>
	/// The GameObject sending this request.
	/// </summary>
	public readonly GameObject caller;
	
	/// <summary>
	/// Was the attack evaded?
	/// </summary>
	public readonly bool wasEvaded;
	
	/// <summary>
	/// The damage performed by the attack.
	/// </summary>
	public readonly int damage;
	
	public UnitAttackResponse(GameObject caller, bool wasEvaded, int damage) {
		this.caller = caller;
		this.wasEvaded = wasEvaded;
		this.damage = damage;
	}
}
