using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	public KeyCode ActivationKeyCode;
	public GameObject ActivationPanel;
	public float Acceleration;
	public float Mass;
	public Vector3 Direction;

	private GameObject playerCharacter;
	private ConstantForce cforce;

	private enum MOTION_STATE
	{
		AT_REST,
		POSITIVE,
		NEGATIVE
	}

	private MOTION_STATE currentMotionState;

	// Use this for initialization
	void Start () {
		playerCharacter = GameObject.Find ("Character");
		cforce = gameObject.AddComponent<ConstantForce> ();
		currentMotionState = MOTION_STATE.AT_REST;
	}

	void FixedUpdate () {
		if (Input.GetKey (ActivationKeyCode))
		{
			Vector3 forwardRay = playerCharacter.transform.forward * 10.0f;
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (playerCharacter.transform.position, forwardRay, out hit, 20.0f)) {
				if (hit.collider.name == ActivationPanel.name && !IsInMotion()) {
					Debug.Log ("HIT /w name!!!!");
					cforce.force = Direction * Mass * Acceleration;
					gameObject.GetComponent<Rigidbody> ().isKinematic = false;
					currentMotionState = MOTION_STATE.POSITIVE;
				} else {
					Debug.Log ("HIT /w NO NAME!!!");
				}

			} else {
				Debug.Log ("NO HIT!!");
			}
		}
	}

	private bool IsInMotion() {
		return currentMotionState != MOTION_STATE.AT_REST;
	}
}
