using UnityEngine;
using System.Collections;

public class BattleGUI : MonoBehaviour {
	
	/// <summary>
	/// The possible views of the BattleGUI object. Possible states:
	/// 1. WaitingForEnemyTurn - it is not the player's turn.
	/// 2. Initial - the primary options to show at the beginning of the player's turn.
	/// 3. Moving - the player has selected to move the unit
	/// </summary>
	public enum View {WaitingForEnemyTurn, Initial, Moving};
	
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
	
	// Use this for initialization
	void Start () {
		currentView = View.Initial;
		
		GUILabelHeight = 22f;
		
		GUIAreaHeight = Screen.height / 3;
		GUIAreaWidth = Screen.width / 2;
		GUIAreaLeft = 0f;
		GUIAreaTop = 2 * GUIAreaHeight;
		GUIArea = new Rect(GUIAreaLeft, GUIAreaTop, GUIAreaWidth, GUIAreaWidth);
		
		titleAreaHeight = GUILabelHeight;
		titleAreaWidth = GUIAreaWidth * 0.80f;
		titleAreaLeft = GUIAreaLeft;
		titleAreaTop = GUIAreaTop;
		titleArea = new Rect(titleAreaLeft, titleAreaTop, titleAreaWidth, titleAreaHeight);	
		
		buttonHeight = 30f;
		buttonWidth = GUIAreaWidth * 0.35f;
		buttonLeft = GUIAreaLeft + buttonWidth / 6;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		switch (currentView) {
		case View.WaitingForEnemyTurn:
			WaitingForEnemyTurnGUI();
			break;
			
		case View.Initial:
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
	
	private void WaitingForEnemyTurnGUI() {
		GUI.Box(GUIArea, string.Empty);
	}
	
	private void InitialTurnGUI() {
		GUI.Box(GUIArea, string.Empty);
		GUI.Label(titleArea, "Turn Options");
		
		float currentButtonHeight = titleAreaTop + 3 * GUILabelHeight;
		
		Rect moveButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		GUI.Button(moveButtonRect, "Move Unit");
		currentButtonHeight += buttonHeight;
		
		Rect attackButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		GUI.Button(attackButtonRect, "Attack");
		currentButtonHeight += buttonHeight;
		
		Rect finishTurnButtonRect = new Rect(buttonLeft, currentButtonHeight, buttonWidth, buttonHeight);
		GUI.Button(finishTurnButtonRect, "Finish Turn");
		currentButtonHeight += buttonHeight;
	}
	
	private void MovingGUI() {
		GUI.Box(GUIArea, string.Empty);
	}
}
