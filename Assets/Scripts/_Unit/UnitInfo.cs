using UnityEngine;
using System.Collections;

/// <summary>
/// Information about this unit's controller, stats, equipment, status, etc.
/// </summary>
public class UnitInfo : MonoBehaviour {
	
	/// <summary>
	/// Who controls this unit?
	/// </summary>
	public AbstractController controller;
	
	public float walkingDistance = 10f;
	
	// Use this for initialization
	void Start () {
	
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
}
