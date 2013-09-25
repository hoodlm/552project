using UnityEngine;
using System.Collections;

/// <summary>
/// Tests that a unit can respond to a turn scheduler. The unit will turn red for 2 seconds, then end its turn.
/// </summary>
public class UnitTurnTest : MonoBehaviour {
	
	/// <summary>
	/// The scheduler that most recently gave this unit its turn.
	/// </summary>
	private GameObject Scheduler;
	private bool MyTurn;
	private float TimeLeftInTurn;
	
	// Use this for initialization
	void Start () {
		MyTurn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (MyTurn) {
			DuringTurn();
		}
	}
	
	/// <summary>
	/// Tasks for this Unit to perform at the start of its turn.
	/// </summary>
	/// <param name='scheduler'>
	/// Scheduler.
	/// </param>
	public void StartTurn(GameObject scheduler) {
		Debug.Log(this.name + " is starting its turn.");
		
		MyTurn = true;
		this.Scheduler = scheduler;
		renderer.material.color = Color.red;
		TimeLeftInTurn = 2f;
	}
	
	/// <summary>
	/// Tasks for this Unit to perform during its turn.
	/// </summary>
	private void DuringTurn() {
		TimeLeftInTurn -= Time.deltaTime;
		if (TimeLeftInTurn <= 0f) {
			FinishTurn();
		}
	}
	
	/// <summary>
	/// Tasks for this Unit to perform at the end of its turn.
	/// </summary>
	private void FinishTurn() {
		Debug.Log(this.name + " has finished its turn.");
		
		renderer.material.color = Color.white;
		MyTurn = false;
		Scheduler.SendMessage("NextTurn");
	}
}
