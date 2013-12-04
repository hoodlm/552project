using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper class to include information about the outcome of a battle.
/// </summary>
public class BattleResult {

	public readonly string victor;
	public readonly int unitsLeft;
	
	public BattleResult(string victor, int unitsLeft) {
		this.victor = victor;
		this.unitsLeft = unitsLeft;
	}
}