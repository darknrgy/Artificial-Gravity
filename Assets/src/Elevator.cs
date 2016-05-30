using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	public KeyCode ActivationKeyCode;
	public GameObject ActivationPanel;
	public float Acceleration;
	public Vector3 Direction;
	public float MaximumDistance;
	public float DistanceUntilBreak;
	public bool DebugDraw;

	private GameObject playerCharacter;
	private ConstantForce cforce;

	private enum MOTION_STATE
	{
		AT_REST,
		IN_MOITION
	}

	private MOTION_STATE currentMotionState;

	private Vector3 forceStep;
	private const int forceInterationMaximum = 1024;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private int iterationCount = 0;

	// Use this for initialization
	void Start () {
		playerCharacter = GameObject.Find ("Character");
		cforce = gameObject.GetComponent<ConstantForce> ();
		currentMotionState = MOTION_STATE.AT_REST;
		startPosition = gameObject.transform.position;
		endPosition = -((Direction * MaximumDistance) + startPosition); 

	}

	void FixedUpdate () {
		if (DebugDraw) {
			Debug.DrawRay (startPosition, endPosition, Color.blue);
		}

		if (Input.GetKey (ActivationKeyCode))
		{
			Vector3 forwardRay = playerCharacter.transform.forward * 10.0f;
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (playerCharacter.transform.position, forwardRay, out hit, 20.0f)) {
				if (hit.collider.name == ActivationPanel.name && !IsInMotion()) {
					cforce.relativeForce = Direction * gameObject.GetComponent<Rigidbody>().mass * Acceleration;
					forceStep = cforce.relativeForce / forceInterationMaximum;
					currentMotionState = MOTION_STATE.IN_MOITION;
				}
			}
		}

		if (IsInMotion()) {
			float distanceToEnd = Vector3.Distance (gameObject.transform.position, endPosition);
			if (distanceToEnd <= DistanceUntilBreak) {
				cforce.relativeForce -= forceStep;
				iterationCount++;

				if (iterationCount == forceInterationMaximum) {
					currentMotionState = MOTION_STATE.AT_REST;
					iterationCount = 0;
				}
			}
		}
	}

	public bool IsInMotion() {
		return currentMotionState != MOTION_STATE.AT_REST;
	}
}
