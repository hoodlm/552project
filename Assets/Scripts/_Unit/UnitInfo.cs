using UnityEngine;
using System.Collections;
using System.Text;

/// <summary>
/// Information about this unit's controller, stats, equipment, status, etc.
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
	
	
	// Use this for initialization
	void Start () {
		currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Calculates the max distance this unit can move in a turn.
	/// </summary>
	public float CalculateWalkingDistance() {
		return walkingDistance;
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
