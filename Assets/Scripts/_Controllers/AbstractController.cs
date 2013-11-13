using UnityEngine;
using System.Collections;

/// <summary>
/// A controller is a Player or AI that controls Units on the battlefield.
/// The Unit will notify its Controller when it needs orders, and the Controller
/// then sends orders to the Unit.
/// </summary>
public abstract class AbstractController : MonoBehaviour {
	
	public Color teamColor;
	
	protected GameObject scheduler;
	
	/// <summary>
	/// Whether this controller is currently giving actions.
	/// </summary>
	protected bool inAction;
	
	/// <summary>
	/// The unit currently receiving actions from this controller.
	/// </summary>
	public GameObject currentUnit;
	
	/// <summary>
	/// The current unit is actively trying to complete a move.
	/// </summary>
	protected bool isMoving;
	
	/// <summary>
	/// The current unit has already (successfully) moved.
	/// </summary>
	public bool hasAlreadyMoved;
	
	/// <summary>
	/// The ground or terrain of this level.
	/// </summary>
	protected GameObject ground;
	
	protected GameObject player;
	
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindGameObjectWithTag("Ground");
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	/// <summary>
	/// Asks the controller to take control of a unit and switch to an Active state.
	/// </summary>
	public void TakeControlOf(GameObject unit) {
		if (!inAction) {
			Debug.Log(this.name + " is taking control of " + unit.name);
			
			this.BroadcastMessage("ChangeGUIState", "StartTurn", SendMessageOptions.DontRequireReceiver);
			inAction = true;
			currentUnit = unit;
			hasAlreadyMoved = false;
			isMoving = false;
			currentUnit.SendMessage("BeginTurn", this.gameObject, SendMessageOptions.RequireReceiver);
			player.SendMessage("AutoMoveCameraTo", currentUnit.transform.position, SendMessageOptions.DontRequireReceiver);
			currentUnit.SendMessage("ShowActiveUnit", SendMessageOptions.DontRequireReceiver);
		} else {
			Debug.LogWarning("Method \"TakeControlOf\" was unexpectedly called while the controller is already active.");
		}
	}
	
	/// <summary>
	/// Requests that this controller release control of its unit and switch to an Inactive state.
	/// </summary>
	public void GiveUpControl() {
		if (inAction) {
			Debug.Log(this.name + " is giving up control of " + currentUnit.name);
			inAction = false;
			currentUnit.SendMessage("FinishTurn", this.gameObject, SendMessageOptions.RequireReceiver);
			currentUnit = null;
			scheduler.SendMessage("NextTurn", SendMessageOptions.RequireReceiver);
		} else {
			Debug.LogWarning("Method \"GiveUpControl\" was unexpectedly called while the controller is not active.");
		}
	}
	
	/// <summary>
	/// Whether this controller is active - i.e. if it is controlling the active unit.
	/// </summary>
	public bool IsInAction() {
		return this.inAction;
	}
	
	/// <summary>
	/// Moves the unit that this controller is currently controlling.
	/// </summary>
	protected abstract void SendMoveOrderToUnit(Vector3 target);
	
	/// <summary>
	/// Notify this controller than the current unit has finished its current move order.
	/// </summary>
	protected abstract void UnitDoneMoving(UnitMoveResponse response);
	
	/// <summary>
	/// Tells the current unit to attack target.
	/// </summary>
	protected abstract void SendAttackOrderToUnit(GameObject target);
	
	/// <summary>
	/// Notify this controller than the current unit has finished its current attack order.
	/// </summary>
	protected abstract void UnitDoneAttacking(UnitAttackResponse response);
}
