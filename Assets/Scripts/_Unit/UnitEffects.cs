using UnityEngine;
using System.Collections;

/// <summary>
/// A script to handle miscellaneous unit visual effects, such as the movement radius display.
/// </summary>
public class UnitEffects : MonoBehaviour {
	
	// Movement range effect
	public GameObject movementRadiusPrefab;
	public Color movementRadiusColor;
	private float movementRadiusHeight = 2.0f;
	private GameObject activeMovementRadiusObject;
	private bool showingRadius;
	
	// Highlight active unit effect
	public GameObject activeUnitPrefab;
	private GameObject activeUnitHighlightObject;
	private bool showingActiveUnit;
	
	
	// Use this for initialization
	void Start () {
		showingRadius = false;
		showingActiveUnit = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowMovementRadius() {
		float radius = this.GetComponent<UnitInfo>().CalculateWalkingDistance();
		ShowMovementRadius(radius);
	}
	
	public void ShowMovementRadius(float radius) {
		if (!showingRadius) {
			showingRadius = true;
			activeMovementRadiusObject = Instantiate(movementRadiusPrefab, transform.position, Quaternion.identity) as GameObject;
			activeMovementRadiusObject.transform.Rotate(Vector3.left, 90f);
			activeMovementRadiusObject.transform.localScale = new Vector3(radius, radius, movementRadiusHeight);
			activeMovementRadiusObject.renderer.material.color = movementRadiusColor;
		}
	}
	
	public void HideMovementRadius() {
		showingRadius = false;
		Destroy(activeMovementRadiusObject);
	}
	
	public void ShowActiveUnit() {
		if (!showingActiveUnit) {
			showingActiveUnit = true;
			activeUnitHighlightObject = Instantiate(activeUnitPrefab, transform.position, Quaternion.identity) as GameObject;
		}
	}
	
	public void HideActiveUnit() {
		showingActiveUnit = false;
		Destroy(activeUnitHighlightObject);
	}
}
