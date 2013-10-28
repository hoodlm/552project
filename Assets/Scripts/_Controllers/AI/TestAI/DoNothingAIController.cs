using UnityEngine;
using System.Collections;

public class DoNothingAIController : AbstractController {
	
	/// <summary>
	/// How long to delay before ending turn.
	/// </summary>
	public float timeToEnd = 1.0f;
	private float timer;
	private bool timerStarted;
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = GameObject.FindGameObjectWithTag("Player");
		timerStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (inAction && !timerStarted) {
			timer = timeToEnd;
			timerStarted = true;
		} else if (inAction && timerStarted) {
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
				GiveUpControl();
			}
		}		
	}
	
	/// <summary>
	/// Since the AI never moves the unit, this method body is empty.
	/// </summary>
	override protected void SendMoveOrderToUnit(Vector3 target) {
			
	}
	
	/// <summary>
	/// Since the AI never moves the unit, this method body is empty.
	/// </summary>
	override protected void UnitDoneMoving(UnitMoveResponse response) {
		
	}
}
