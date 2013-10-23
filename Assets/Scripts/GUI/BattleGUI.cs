using UnityEngine;
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
	public enum View {WaitingForEnemyTurn, StartTurn, Moving};
	
	public View currentView;
	public bool hasMoved;
	
	private float GUILabelHeight;
	
	private float GUIAreaWidth;
	private float GUIAreaHeight;
	private float GUIAreaLeft;
	private float GUIAreaTop;
	private Rect GUIArea;
	
	private float titleAreaWidth;
	private float titleAreaHeight;
	private float titleAreaLeft;
	private float titleAreaTop;
	private Rect titleArea;
	
	private float buttonLeft;
	private float buttonHeight;
	private float buttonWidth;
	
	private GameObject ground;
	private AbstractController playerController;
	
	// Use this for initialization
	void Start () {
		currentView = View.StartTurn;
		
		GUILabelHeight = 22f;
		
		GUIAreaHeight = Screen.height / 3;
		GUIAreaWidth = Screen.width / 2;
		GUIAreaLeft = 0f;
		GUIAreaTop = 0f;
		GUIArea = new Rect(GUIAreaLeft, GUIAreaTop, GUIAreaWidth, GUIAreaHeight);
		
		titleAreaHeight = GUILabelHeight;
		titleAreaWidth = GUIAreaWidth * 0.80f;
		titleAreaLeft = GUIAreaLeft;
		titleAreaTop = GUIAreaTop;
		titleArea = new Rect(titleAreaLeft, titleAreaTop, titleAreaWidth, titleAreaHeight);	
		
		buttonHeight = 30f;
		buttonWidth = GUIAreaWidth * 0.35f;
		buttonLeft = GUIAreaLeft + buttonWidth / 6;
		
		ground = GameObject.FindWithTag("Ground");
		playerController = this.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		switch (currentView) {
		case View.WaitingForEnemyTurn:
			WaitingForEnemyTurnGUI();
			break;
			
		case View.StartTurn:
			InitialTurnGUI();
			break;
			
		case View.Moving:
			MovingGUI();
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
			
		default:
			string debugString = string.Format("Unrecognized state change requested \"{0}\"", state);
			Debug.LogError(debugString);
			break;
		}
	}
	
	private void WaitingForEnemyTurnGUI() {
		GUI.Box(GUIArea, string.Empty);
	}
	
	private void InitialTurnGUI() {
		
		GUI.Box(GUIArea, string.Empty);
		GUI.Label(titleArea, "Turn Options");
		
		float currentButtonHeight = titleAreaTop + 3 * GUILabelHeight;
		
		// MOVEMENT
		Rect moveButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		if (GUI.Button(moveButtonRect, "Move Unit") || Input.GetKeyDown(KeyCode.M)) {
			playerController.currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
			currentView = View.Moving;
		}
		currentButtonHeight += buttonHeight;
		
		// ATTACK
		Rect attackButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		GUI.Button(attackButtonRect, "Attack");
		currentButtonHeight += buttonHeight;
		
		// FINISH TURN
		Rect finishTurnButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		if (GUI.Button(finishTurnButtonRect, "Finish Turn") || Input.GetKeyDown(KeyCode.F)) {
			playerController.currentUnit.SendMessage("HideActiveUnit", SendMessageOptions.RequireReceiver);
			currentView = View.WaitingForEnemyTurn;
			playerController.SendMessage("GiveUpControl", SendMessageOptions.RequireReceiver);
		}
		currentButtonHeight += buttonHeight;
	}
	
	private void MovingGUI() {
		
		playerController.currentUnit.SendMessage("ShowMovementRadius", SendMessageOptions.RequireReceiver);
		
		GUI.Box(GUIArea, string.Empty);
		if (Input.GetButtonDown ("Fire1")) {
			Vector3 target = GetCursorPositionOnTerrain();
			BroadcastMessage("SendMoveOrderToUnit", target, SendMessageOptions.RequireReceiver);
			playerController.currentUnit.SendMessage("HideMovementRadius", SendMessageOptions.RequireReceiver);
			currentView = View.StartTurn;
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
}
