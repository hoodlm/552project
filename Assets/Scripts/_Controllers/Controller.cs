using UnityEngine;
using System.Collections;

/// <summary>
/// A controller is a Player or AI that controls Units on the battlefield.
/// The Unit will notify its Controller when it needs orders, and the Controller
/// then sends orders to the Unit.
/// </summary>
public abstract class Controller : MonoBehaviour {
	
	/// <summary>
	/// Whether this controller is currently giving actions.
	/// </summary>
	protected bool Active;
	
	/// <summary>
	/// The unit currently receiving actions from this controller.
	/// </summary>
	protected GameObject CurrentUnit;
	
	/// <summary>
	/// Asks the controller to take control of a unit and switch to an Active state.
	/// </summary>
	public void TakeControlOf(GameObject unit) {
		Active = true;
		CurrentUnit = unit;
	}
	
	/// <summary>
	/// Requests that this controller release control of its unit and switch to an Inactive state.
	/// </summary>
	public void GiveUpControl() {
		CurrentUnit = null;
		Active = false;
	}
	
	/// <summary>
	/// Whether this controller is active.
	/// </summary>
	public bool IsActive() {
		return this.Active;
	}
	
	/// <summary>
	/// Moves the unit that this controller is currently controlling.
	/// </summary>
	protected abstract void MoveUnit();
}
