using UnityEngine;
using System.Collections;

/// <summary>
/// A script to handle miscellaneous unit visual effects, such as the movement radius display.
/// </summary>
public class UnitEffects : MonoBehaviour {
	
	public GameObject movementRadiusPrefab;
	public Color movementRadiusColor;
	private float movementRadiusHeight = 3.0f;
	private GameObject activeMovementRadiusObject;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowMovementRadius(float radius) {
		activeMovementRadiusObject = Instantiate(movementRadiusPrefab, transform.position, Quaternion.identity) as GameObject;
		activeMovementRadiusObject.transform.Rotate(Vector3.left, 90f);
		activeMovementRadiusObject.transform.localScale = new Vector3(radius, radius, movementRadiusHeight);
		activeMovementRadiusObject.renderer.material.color = movementRadiusColor;
	}
	
	public void HideMovementRadius() {
		Destroy(activeMovementRadiusObject);
	}
}
