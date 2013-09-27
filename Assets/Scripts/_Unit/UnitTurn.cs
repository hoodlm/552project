using UnityEngine;
using System.Collections;

public class UnitTurn : MonoBehaviour {
	
	/// <summary>
	/// Who is controlling this unit?
	/// </summary>
	public GameObject controller;
	
	/// <summary>
	/// The scheduler that most recently gave this unit its turn.
	/// </summary>
	private GameObject scheduler;
	
	private bool myTurn = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (myTurn) {
			DuringTurn();
		}
	}
	
	/// <summary>
	/// Tasks for this Unit to perform at the start of its turn.
	/// </summary>
	/// <param name='scheduler'>
	/// Scheduler.
	/// </param>
	public void BeginTurn(GameObject scheduler) {
		if (!myTurn) {
			Debug.Log(this.name + " is starting its turn.");
			myTurn = true;
			this.scheduler = scheduler;
			controller.SendMessage("TakeControlOf", this.gameObject, SendMessageOptions.RequireReceiver);
		}
	}
	
	/// <summary>
	/// Tasks for this Unit to perform during its turn.
	/// </summary>
	private void DuringTurn() {
		
	}
	
	/// <summary>
	/// Tasks for this Unit to perform at the end of its turn.
	/// </summary>
	private void FinishTurn() {
		if (myTurn) {
			Debug.Log(this.name + " has finished its turn.");
			myTurn = false;
			controller.SendMessage("GiveUpControl", SendMessageOptions.RequireReceiver);
			scheduler.SendMessage("NextTurn", SendMessageOptions.RequireReceiver);
		}
	}
}
