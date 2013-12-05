using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the GUI and Player I/O during pre battle phase.
/// </summary>
public class WorldMapGui : MonoBehaviour {
	public enum WorldView {Armory, Ace, Berg, Clark, Dale, Menu};
	
	public WorldView currentWorldView;
	void OnGUI () {
		switch (currentWorldView) {
		case WorldView.Armory:
			Armory();
			break;
			
		case WorldView.Ace:
			AceSkillTree();
			break;
			
		case WorldView.Berg:
			BergSkillTree();
			break;
			
		case WorldView.Clark:
			ClarkSkillTree();
			break;
			
		case WorldView.Dale:
			DaleSkillTree();
			break;
			
		default:
			MenuGUI();
			break;
		}
	}
	void MenuGUI(){
		this.currentWorldView = WorldView.Menu;
		// Make a background box
		//GUI.Box(new Rect(100,100,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,200,20), "Level 1: The Invasion Begins")) {
			Application.LoadLevel(1);
		}

		// Make the second button.
		if(GUI.Button(new Rect(20,70,200,20), "Level 2: The Sinking Forest")) {
			Application.LoadLevel(2);
		}
		if(GUI.Button(new Rect(20,100,200,20), "SideQuest: Twin Bridges")) {
			Application.LoadLevel(3);
		}
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Armory")){
			Armory ();
		}
	}
	void Armory(){
		this.currentWorldView = WorldView.Armory;
		if(GUI.Button(new Rect(20,40,200,20), "Ace")) {
			AceSkillTree();
		}

		// Make the second button.
		if(GUI.Button(new Rect(20,70,200,20), "Berg")) {
			BergSkillTree();
		}
		if(GUI.Button(new Rect(20,100,200,20), "Clark")) {
			ClarkSkillTree();
		}
		if(GUI.Button(new Rect(20,130,200,20), "Dale")) {
			DaleSkillTree();
		}
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Back")){
			MenuGUI();
		}
		
	}
	//add a box with gui text explaining the characters strengths and weaknesses
	void AceSkillTree(){
		this.currentWorldView = WorldView.Ace;
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Back")){
			Armory();
		}
	}
	void BergSkillTree(){
		this.currentWorldView = WorldView.Berg;
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Back")){
			Armory();
		}
	}
	void ClarkSkillTree(){
		this.currentWorldView = WorldView.Clark;
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Back")){
			Armory();
		}
	}
	void DaleSkillTree(){
		this.currentWorldView = WorldView.Dale;
		if (GUI.Button (new Rect (20,Screen.height - 50,200,50), "Back")){
			Armory();
		}
	}
	
}