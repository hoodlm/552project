﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the GUI and Player I/O during battles.
/// </summary>
public class BattleGUI : MonoBehaviour {
	
	/// <summary>
	/// The possible views of the BattleGUI object. Possible states:
	/// 1. WaitingForEnemyTurn - it is not the player's turn.
	/// 2. StartTurn - the primary options to show at the beginning of the player's turn.
	/// 3. Moving - the player has selected to move the unit
	/// </summary>
	public enum View {WaitingForEnemyTurn, StartTurn, Moving, Attacking, Disabled};
	
	public View currentView;
	
	private float GUILabelHeight;
	
	private float GUIAreaWidth;
	private float GUIAreaHeight;
	private float GUIAreaLeft;
	private float GUIAreaTop;
	private Rect GUIArea;
	
	private float unitInfoAreaWidth;
	private float unitInfoAreaHeight;
	private float unitInfoAreaLeft;
	private float unitInfoAreaTop;
	private Rect unitInfoArea;
	
	private float buttonTop;
	private float buttonHeight;
	private float buttonWidth;
	
	private GameObject ground;
	private AbstractController playerController;
	private SimpleTurnScheduler turnScheduler;
	
	// We need state variables for keyboard presses because
	// these are normally set on a per-frame basis,
	// but OnGUI may be called several times per frame.
	private bool moveKeyPressed;
	private bool endTurnKeyPressed;
	private bool attackKeyPressed;
	
	// Use this for initialization
	void Start () {
		currentView = View.StartTurn;
		
		GUILabelHeight = 22f;
		
		GUIAreaHeight = Screen.height / 3;
		GUIAreaWidth = 7 * Screen.width / 12;
		GUIAreaLeft = 0f;
		GUIAreaTop = 0f;
		GUIArea = new Rect(GUIAreaLeft, GUIAreaTop, GUIAreaWidth, GUIAreaHeight);
		
		unitInfoAreaHeight = GUILabelHeight * 4.0f;
		unitInfoAreaWidth = GUIAreaWidth * 0.80f;
		unitInfoAreaLeft = GUIAreaLeft;
		unitInfoAreaTop = GUIAreaTop;
		unitInfoArea = new Rect(unitInfoAreaLeft, unitInfoAreaTop, unitInfoAreaWidth, unitInfoAreaHeight);	
		
		buttonHeight = 30f;
		buttonWidth = GUIAreaWidth * 0.20f;
		buttonTop = GUIAreaTop + GUIAreaHeight - buttonHeight;
		
		ground = GameObject.FindWithTag("Ground");
		playerController = this.GetComponent<PlayerController>();
		turnScheduler = GameObject.FindWithTag("BattleManager").GetComponent<SimpleTurnScheduler>();
	}
	
	// Update is called once per frame
	void Update () {
		// We want to get these at the beginning of the frame. Anytime OnGUI reads a true,
		// it will clear them to false so that multiple key presses aren't recorded in a single frame.
		//moveKeyPressed = Input.GetKeyDown(KeyCode.M);
		//endTurnKeyPressed = Input.GetKeyDown(KeyCode.E);
		//attackKeyPressed = Input.GetKeyDown(KeyCode.A);
	}
	
	void OnGUI() {
		
		switch (currentView) {
		case View.WaitingForEnemyTurn:
			WaitingForEnemyTurnGUI();
			break;
			
		case View.StartTurn:
			if (null != playerController.currentUnit)
				InitialTurnGUI();
			break;
			
		case View.Moving:
			MovingGUI();
			break;
			
		case View.Attacking:
			AttackingGUI();
			break;
			
		case View.Disabled:
			//
			break;
			
		default:
			WaitingForEnemyTurnGUI();
			break;
		}
	}
	
	/// <summary>
	/// Allow external classes to change the state of the GUI.
	/// </summary>
	public void ChangeGUIState(string state) {
		Debug.Log("Changing battle GUI state to " + state);
		
		switch (state) {
			
		case "StartTurn":
			this.currentView = View.StartTurn;
			break;
			
		case "WaitingForEnemyTurn":
			this.currentView = View.WaitingForEnemyTurn;
			break;
			
		case "Moving":
			this.currentView = View.Moving;
			break;
			
		case "Attacking":
			this.currentView = View.Attacking;
			break;
			
		case "Disable":
			this.currentView = View.Disabled;
			break;
			
		default:
			string debugString = string.Format("Unrecognized state change requested \"{0}\"", state);
			Debug.LogError(debugString);
			break;
		}
	}
	
	private void WaitingForEnemyTurnGUI() {
		GUI.Box(GUIArea, string.Empty);
		GameObject currentUnit = turnScheduler.WhoseTurn();
		string infoString = "";
		if (null != currentUnit)
			infoString = turnScheduler.WhoseTurn().GetComponent<UnitInfo>().GetInfoString();
		GUI.Label(unitInfoArea, infoString);
	}
	
	private void InitialTurnGUI() {
		
		GUI.Box(GUIArea, string.Empty);
		string infoString = playerController.currentUnit.GetComponent<UnitInfo>().GetInfoString();
		GUI.Label(unitInfoArea, infoString);
		
		float currentButtonLeft = GUIAreaLeft + buttonWidth / 2;
		
		// MOVEMENT (hidden if the player has already moved)
		if (!playerController.hasAlreadyMoved) {
			Rect moveButtonRect = new Rect(currentButtonLeft, buttonTop, buttonWidth, buttonHeight);
			if (GUI.Button(moveButtonRect, "Move") || moveKeyPressed) {
				Debug.Log("Player is moving the unit through GUI.");
				moveKeyPressed = false;
				playerController.currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
				ChangeGUIState("Moving");
			}
			currentButtonLeft += buttonWidth;
		}
		
		// ATTACK (hidden if the player has already attacked)
		if (!playerController.hasAlreadyAttacked) {
			Rect attackButtonRect = new Rect(currentButtonLeft, buttonTop, buttonWidth, buttonHeight);
			if (GUI.Button(attackButtonRect, "Attack") || attackKeyPressed) {
				Debug.Log ("Player is attacking through GUI.");
				attackKeyPressed = false;
				playerController.currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
				ChangeGUIState("Attacking");
			}
			currentButtonLeft += buttonWidth;
		}
		
		// FINISH TURN
		Rect finishTurnButtonRect = new Rect(currentButtonLeft, buttonTop, buttonWidth, buttonHeight);
		if (GUI.Button(finishTurnButtonRect, "End Turn") || endTurnKeyPressed) {
			Debug.Log("Player is ending turn through GUI.");
			endTurnKeyPressed = false;
			playerController.currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
			ChangeGUIState("WaitingForEnemyTurn");
			playerController.SendMessage("GiveUpControl", SendMessageOptions.RequireReceiver);
		}
		currentButtonLeft += buttonWidth;
	}
	
	private void MovingGUI() {
		
		playerController.currentUnit.SendMessage("ShowMovementRadius", SendMessageOptions.RequireReceiver);
		
		GUI.Box(GUIArea, string.Empty);
		if (Input.GetButtonDown ("Fire1")) {
			Vector3 target = GetCursorPositionOnTerrain();
			BroadcastMessage("SendMoveOrderToUnit", target, SendMessageOptions.RequireReceiver);
			playerController.currentUnit.SendMessage("HideMovementRadius", SendMessageOptions.RequireReceiver);
		}
		
	}
	
	private void AttackingGUI() {
		
		playerController.currentUnit.SendMessage("ShowAttackRadius", SendMessageOptions.RequireReceiver);
		GUI.Box(GUIArea, string.Empty);
		if (Input.GetButtonDown ("Fire1")) {
			GameObject target = GetUnitAtCursor();
			if (target != null && target.GetComponent<UnitInfo>() != null) {
				playerController.SendMessage("SendAttackOrderToUnit", target, SendMessageOptions.RequireReceiver);
				playerController.currentUnit.SendMessage("HideAttackRadius", SendMessageOptions.RequireReceiver);
			} else {
				playerController.currentUnit.SendMessage("HideAttackRadius", SendMessageOptions.RequireReceiver);
				currentView = View.StartTurn;
			}
		}
		
	}
	
	/// <summary>
	/// Helper method to retrieve the cursor position on terrain.
	/// </summary>
	/// <returns>
	private Vector3 GetCursorPositionOnTerrain() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		ground.collider.Raycast(ray, out hit, float.PositiveInfinity);
		return hit.point;
	}
	
	/// <summary>
	/// Helper method to retrieve the unit at cursor.
	/// </summary>
	/// <returns>
	/// The unit at cursor.
	/// </returns>
	private GameObject GetUnitAtCursor() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
			if (hit.collider.gameObject.tag.Equals("Unit")) {
				return hit.collider.gameObject;
			}
		}
		return null;
	}
}
