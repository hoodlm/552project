using UnityEngine;
using System.Collections;

public class UnitAttacking : MonoBehaviour {
	
	/// <summary>
	/// This unit's stats, primarily used to calculate the range that this unit can move in a single turn.
	/// </summary>
	private UnitInfo unitInfo;
	
	/// <summary>
	/// The most recently received attackRequest.
	/// </summary>
	private UnitAttackRequest attackRequest;
	
	// Use this for initialization
	void Start () {
		unitInfo = this.GetComponent<UnitInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ExecuteAttack(UnitAttackRequest request) {
		attackRequest = request;
		if (request.target.GetComponent<UnitInfo>() == null) {
			BroadcastMessage("DoneAttacking", NotAUnitResponse(), SendMessageOptions.RequireReceiver);
		} else if (request.target.GetComponent<UnitInfo>().controller
			== gameObject.GetComponent<UnitInfo>().controller) {
			
			string debugString = string.Format
					("Attack target is on the same team as attacker!");
			Debug.Log(debugString);
			BroadcastMessage("DoneAttacking", TeammateResponse(), SendMessageOptions.RequireReceiver);
				
		} else if (!withinRange(request.target)) {
			string debugString = string.Format
					("Attack target was distance {0}, but unit's max attack range is {1}",
						(this.transform.position - request.target.transform.position).magnitude,
						unitInfo.CalculateAttackRange());
			Debug.Log(debugString);
			BroadcastMessage("DoneAttacking", OutOfRangeResponse(), SendMessageOptions.RequireReceiver);
		} else {
			transform.LookAt(attackRequest.target.transform);
			animation.Play("Attack");
			
			float damage = unitInfo.attackDamage;
			
			damage += Random.Range(-damage / 2, damage / 2);
			
			attackRequest.target.BroadcastMessage("TakeDamage", damage, SendMessageOptions.RequireReceiver);
			
			UnitAttackResponse response = new UnitAttackResponse(request.caller, false, damage);
			BroadcastMessage("DoneAttacking", response);
		}
	}
	
	private bool withinRange(GameObject target) {
		float attackRange = unitInfo.CalculateAttackRange();
		
		// We compare based on squared distance for performance reasons
		float distanceSquared = (this.transform.position - target.transform.position).sqrMagnitude;
		return (distanceSquared <= attackRange * attackRange);
	}
	
	/// <summary>
	/// Factory method to build an attack response for an out of range position.
	/// </summary>
	private UnitAttackResponse OutOfRangeResponse() {
		return new UnitAttackResponse(this.attackRequest.caller, false, 0f, false, "out of range");
	}
	
	/// <summary>
	/// Factory method to build an attack response for a target that is not a unit.
	/// </summary>
	private UnitAttackResponse NotAUnitResponse() {
		return new UnitAttackResponse(this.attackRequest.caller, false, 0f, false, "target is not a unit");
	}
	
	/// <summary>
	/// Factory method to build an attack response for a target that is on this unit's team.
	/// </summary>
	private UnitAttackResponse TeammateResponse() {
		return new UnitAttackResponse(this.attackRequest.caller, false, 0f, false, "target is on the same team as attacker!");
	}
}
