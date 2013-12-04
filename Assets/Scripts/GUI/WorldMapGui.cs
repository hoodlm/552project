using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle the GUI and Player I/O during pre battle phase.
/// </summary>
public class WorldMapGui : MonoBehaviour {

	void OnGUI () {
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
			OnGUI();
		}
		
	}
	
	void AceSkillTree(){
		
	}
	void BergSkillTree(){
		
	}
	void ClarkSkillTree(){
		
	}
	void DaleSkillTree(){
		
	}
	
}