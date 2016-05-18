using UnityEngine;
using System.Collections;

public class MagneticShoes : MonoBehaviour {

	public float InitalStrength;
	public float DistanceBeforeShoesTakeEffect;
	public bool DebugDraw;


	private float CurrentStrength;
	// Use this for initialization
	void Start () {
		CurrentStrength = InitalStrength;
	}

	void FixedUpdate () {
		if (Input.GetKeyUp (KeyCode.Comma)) {
			CurrentStrength += 1.0f;
		} else if (Input.GetKeyUp (KeyCode.Period)) {
			CurrentStrength -= 1.0f;
		}
			
		Vector3 dir = -gameObject.transform.up;
		RaycastHit hit = new RaycastHit ();
		if (DebugDraw) {
			Debug.DrawRay (transform.position, dir * DistanceBeforeShoesTakeEffect, Color.green);
		}
			
		if (Physics.Raycast (transform.position, dir, out hit, DistanceBeforeShoesTakeEffect)) {
			float inverseDistanceSquared = 1.0f / Mathf.Max(hit.distance * hit.distance, 0.00001f); // Avoid division by 0
			gameObject.GetComponent<Rigidbody> ().AddForce (dir * CurrentStrength * inverseDistanceSquared);
		}
	}
}
