using UnityEngine;
using System.Collections;

public class TrackElevator : MonoBehaviour {
	// TODO, apply -LastForce at different intervals (4/6s, 5/6s, 6/6s of distance) to next point
	public Vector3[] MotionPath;
	public float[] TargetVelocity;
	public bool DebugDraw;

	private enum MOTION_STATE
	{
		IN_MOTION,
		AT_REST
	}

	private Vector3[] originPath;
	private MOTION_STATE currentMotionState;
	private int currentTargetPoint = 0;
	private Vector3 debugDirection;

	private const float MIN_DIST = 1.0f;

	private Vector3 LastForce = Vector3.zero;

	// Use this for initialization
	void Start () {
		Debug.Assert (MotionPath.Length + 1 == TargetVelocity.Length);
		currentMotionState = MOTION_STATE.AT_REST;
		originPath = new Vector3[MotionPath.Length + 1];
		originPath [0] = gameObject.transform.position;
		for (int i = 0; i < MotionPath.Length; ++i) {
			originPath [i + 1] = MotionPath [i];
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (DebugDraw) {
			for (int i = 1; i < originPath.Length; ++i) {
				Debug.DrawLine (originPath [i - 1], originPath [i], Color.blue);
			}
		}

		if (Input.GetKeyUp (KeyCode.F)) {
			currentTargetPoint = 1;
			currentMotionState = MOTION_STATE.IN_MOTION;
			moveTowardsPoint (originPath [currentTargetPoint]);
			Debug.Log ("Activating Elevator based on player action...");
		}

		if (currentMotionState == MOTION_STATE.IN_MOTION
			&& Vector3.Distance(gameObject.transform.position, originPath[currentTargetPoint]) < MIN_DIST) {

			Debug.Log ("Elevator is within proximity of destination point, relocating...");
			currentTargetPoint++;
			if (currentTargetPoint >= originPath.Length) {
				currentTargetPoint = 0;
				currentMotionState = MOTION_STATE.AT_REST;
				gameObject.GetComponent<Rigidbody> ().AddForce (-LastForce);
			} else {
				Debug.Log ("Moving to next target...");
				moveTowardsPoint (originPath [currentTargetPoint]);
			}
		}

		if (currentMotionState == MOTION_STATE.IN_MOTION) {
			float distanceUntilNextPoint = Vector3.Distance(gameObject.transform.position, originPath[currentTargetPoint]);
			Debug.Log (string.Format ("Logging distance until next target {0}", distanceUntilNextPoint));
			Debug.Log (string.Format ("Logging value of current velocity {0}", gameObject.GetComponent<Rigidbody> ().velocity));
		}
			
		if (DebugDraw) {
			Debug.DrawRay (gameObject.transform.position, debugDirection, Color.red);
		}


	}

	private void moveTowardsPoint(Vector3 p) {
		Vector3 diff = p - gameObject.transform.position;
		Vector3 force = diff * gameObject.GetComponent<Rigidbody> ().mass * TargetVelocity [currentTargetPoint];
		gameObject.GetComponent<Rigidbody>().AddForce(force);
		gameObject.GetComponent<Rigidbody> ().AddForce (-LastForce);
		LastForce = force;
		if (DebugDraw) {
			debugDirection = diff;
		}
	}
}
