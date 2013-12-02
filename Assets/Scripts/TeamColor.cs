using UnityEngine;
using System.Collections;

public class TeamColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetTeamColor(Color color) {
		this.renderer.materials[1].color = color;
	}
}
