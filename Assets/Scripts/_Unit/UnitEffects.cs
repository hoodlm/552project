using UnityEngine;
using System.Collections;

/// <summary>
/// A script to handle miscellaneous unit visual effects, such as the movement radius display.
/// </summary>
public class UnitEffects : MonoBehaviour {
	
	// Attack damage effect
	public GameObject damageTextPrefab;
	
	// Movement range effect
	public GameObject movementRadiusPrefab;
	public Color movementRadiusColor;
	private float movementRadiusHeight = 10.0f;
	private GameObject activeMovementRadiusObject;
	private bool showingMovementRadius;
	
	// Attack range effect
	public GameObject attackRadiusPrefab;
	public Color attackRadiusColor;
	private float attackRadiusHeight = 10.0f;
	private GameObject activeAttackRadiusObject;
	private bool showingAttackRadius;
	
	// Highlight active unit effect
	public GameObject activeUnitPrefab;
	private GameObject activeUnitHighlightObject;
	private bool showingActiveUnit;
	
	// This unit's team color
	private Color teamColor;
	
	// Use this for initialization
	void Start () {
		showingMovementRadius = false;
		showingActiveUnit = false;
		
		teamColor = GetComponent<UnitInfo>().controller.teamColor;
		teamColor.a = 1.0f;
		attackRadiusColor.a = 1.0f;
		movementRadiusColor.a = 1.0f;
		
		this.gameObject.BroadcastMessage("SetTeamColor", teamColor, SendMessageOptions.DontRequireReceiver);
		//this.gameObject.renderer.material.color = teamColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ShowDamageEffect(int damage) {
		GameObject dmg = Instantiate(damageTextPrefab, transform.position, Quaternion.identity) as GameObject;
		dmg.transform.Rotate(Vector3.right, 90.0f);
		dmg.GetComponent<TextMesh>().text = damage.ToString();
	}
	
	public void ShowMovementRadius() {
		float radius = this.GetComponent<UnitInfo>().CalculateWalkingDistance();
		ShowMovementRadius(radius);
	}
	
	public void ShowMovementRadius(float radius) {
		if (!showingMovementRadius) {
			showingMovementRadius = true;
			activeMovementRadiusObject = Instantiate(movementRadiusPrefab, transform.position, Quaternion.identity) as GameObject;
			activeMovementRadiusObject.transform.Rotate(Vector3.left, 90f);
			activeMovementRadiusObject.transform.localScale = new Vector3(radius, radius, movementRadiusHeight);
			activeMovementRadiusObject.renderer.material.color = movementRadiusColor;
		}
	}
	
	public void HideMovementRadius() {
		showingMovementRadius = false;
		Destroy(activeMovementRadiusObject);
	}
	
	public void ShowAttackRadius() {
		float radius = this.GetComponent<UnitInfo>().CalculateAttackRange();
		ShowAttackRadius(radius);
	}
	
	public void ShowAttackRadius(float radius) {
		if (!showingAttackRadius) {
			radius *= 0.90f;
			showingAttackRadius = true;
			activeAttackRadiusObject = Instantiate(attackRadiusPrefab, transform.position, Quaternion.identity) as GameObject;
			activeAttackRadiusObject.transform.Rotate(Vector3.left, 90f);
			activeAttackRadiusObject.transform.localScale = new Vector3(radius, radius, attackRadiusHeight);
			activeAttackRadiusObject.renderer.material.color = attackRadiusColor;
		}
	}
	
	public void HideAttackRadius() {
		showingAttackRadius = false;
		Destroy(activeAttackRadiusObject);
	}
	
	public void ShowActiveUnit() {
		if (!showingActiveUnit) {
			showingActiveUnit = true;
			activeUnitHighlightObject = Instantiate(activeUnitPrefab, transform.position, Quaternion.identity) as GameObject;
			activeUnitHighlightObject.particleSystem.startColor = Color.Lerp(teamColor, Color.white, 0.6f);
		}
	}
	
	public void HideActiveUnit() {
		showingActiveUnit = false;
		Destroy(activeUnitHighlightObject);
	}
}
