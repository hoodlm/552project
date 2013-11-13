using UnityEngine;
using System.Collections;
using System.Text;

/// <summary>
/// Information about this unit's controller, stats, equipment, status, etc.
/// Analogous to a character sheet.
/// </summary>
public class UnitInfo : MonoBehaviour {
	
	/// <summary>
	/// Who controls this unit?
	/// </summary>
	public AbstractController controller;
	
	public float maxHP = 100f;
	public float currentHP;
	
	public float attackDamage = 10f;
	public float defense = 5f;
	
	public float walkingDistance = 10f;
	public float attackRange = 2f;
	
	
	// Use this for initialization
	void Start () {
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.currentHP <= 0f) {
			KillUnit();
		}
	}
	
	public void KillUnit() {
		gameObject.renderer.material.color = Color.black;
		GameObject.FindGameObjectWithTag("BattleManager").SendMessage("RemoveFromQueue", this.gameObject);
	}
	
	public void TakeDamage(float damage) {
		string info = string.Format("{0} took {1} damage!", this.gameObject.name, damage);
		Debug.Log(info);
		this.currentHP -= damage;
	}
	
	/// <summary>
	/// Calculates the max distance this unit can move in a turn.
	/// </summary>
	public float CalculateWalkingDistance() {
		return walkingDistance;
	}
	
	public float CalculateAttackRange() {
		return attackRange;
	}
	
	/// <summary>
	/// Build an info string for this unit. This is a brief string for display in GUI, etc.
	/// </summary>
	public string GetInfoString() {
		StringBuilder sb = new StringBuilder();
		sb.AppendLine(gameObject.name);
		sb.AppendLine(string.Format("HP: {0}/{1}", currentHP, maxHP));
		
		return sb.ToString();
	}
}
