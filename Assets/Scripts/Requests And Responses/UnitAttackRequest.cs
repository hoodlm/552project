using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper object with the information needed for an attack request sent to a unit.
/// </summary>
public class UnitAttackRequest {
	
	/// <summary>
	/// The GameObject sending this request.
	/// </summary>
	public readonly GameObject caller;
	
	/// <summary>
	/// The target for the attack.
	/// </summary>
	public readonly GameObject target;
	
	/// <summary>
	/// The unit performing the attack.
	/// </summary>
	public readonly GameObject attacker;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UnitAttackRequest"/> class.
	/// </summary>
	/// <param name='caller'>
	/// The object sending the request.
	/// </param>
	/// <param name='target'>
	/// The target of the attack.
	/// </param>
	public UnitAttackRequest(GameObject caller, GameObject target, GameObject attacker) {
		this.target = target;
		this.attacker = attacker;
		this.caller = caller;
	}
	
}
