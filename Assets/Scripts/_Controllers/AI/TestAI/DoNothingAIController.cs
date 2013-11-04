using UnityEngine;
using System.Collections;

public class DoNothingAIController : AbstractAIController {
	
	/// <summary>
	/// How long to delay before ending turn.
	/// </summary>
	public float timeToEnd = 1.0f;
	private float timer;
	private bool timerRunning;
	
	// Use this for initialization
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
		player = GameObject.FindGameObjectWithTag("Player");
		timerRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPhase == TurnPhase.WaitingTurn && this.IsInAction()) {
			currentPhase = TurnPhase.MovingUnit;
			timer = timeToEnd;
			timerRunning = true;
		} else if (currentPhase == TurnPhase.MovingUnit
					&& timerRunning) {
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				timerRunning = false;
				SendMoveOrderToUnit(currentUnit.transform.position);
			}
		} else if (currentPhase == TurnPhase.Attacking) {
			currentPhase = TurnPhase.WaitingTurn;
			currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.DontRequireReceiver);
			GiveUpControl();
		}	
	}
}
