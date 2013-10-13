using UnityEngine;
using System.Collections;

/// <summary>
/// A controller is a Player or AI that controls Units on the battlefield.
/// The Unit will notify its Controller when it needs orders, and the Controller
/// then sends orders to the Unit.
/// </summary>
public abstract class AbstractController : MonoBehaviour {
	
	protected GameObject scheduler;
	
	/// <summary>
	/// Whether this controller is currently giving actions.
	/// </summary>
	protected bool inAction;
	
	/// <summary>
	/// The unit currently receiving actions from this controller.
	/// </summary>
	protected GameObject currentUnit;
	
	/// <summary>
	/// The ground or terrain of this level.
	/// </summary>
	protected GameObject ground;
	
	void Start () {
		scheduler = GameObject.FindGameObjectWithTag("BattleManager");
		ground = GameObject.FindWithTag("Ground");
	}
	
	/// <summary>
	/// Asks the controller to take control of a unit and switch to an Active state.
	/// </summary>
	public void TakeControlOf(GameObject unit) {
		if (!inAction) {
			Debug.Log(this.name + " is taking control of " + unit.name);
			inAction = true;
			currentUnit = unit;
			currentUnit.SendMessage("BeginTurn", this.gameObject, SendMessageOptions.RequireReceiver);
		}
	}
	
	/// <summary>
	/// Requests that this controller release control of its unit and switch to an Inactive state.
	/// </summary>
	public void GiveUpControl() {
		if (inAction) {
			Debug.Log(this.name + " is giving up control of " + currentUnit.name);
			currentUnit.SendMessage("FinishTurn", this.gameObject, SendMessageOptions.RequireReceiver);
			currentUnit = null;
			inAction = false;
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
	protected abstract void SendMoveOrderToUnit();
	
	/// <summary>
	/// Notify this controller than the current unit has finished its current move order.
	/// </summary>
	protected abstract void UnitDoneMoving(UnitMoveResponse response);
}
