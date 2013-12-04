using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {
	
	public float lifetime = 1.50f;
	public float speed = 1.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if (lifetime < 0.0f) {
			GameObject.Destroy(this.gameObject);
		}
		transform.Translate(Vector3.up * speed * Time.deltaTime);
		transform.Translate(Vector3.left * Mathf.Sin(Time.time * 2f) * Time.deltaTime * speed);
	}
}
