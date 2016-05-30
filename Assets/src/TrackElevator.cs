using UnityEngine;
using System.Collections;

public class TrackElevator : MonoBehaviour {

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
	private ConstantForce constForce;
	private Vector3 debugDirection;

	private const float MIN_DIST = 3.0f;

	private const float distanceDivisor = 3;
	private float totalDistanceUntilNextPoint;

	// Use this for initialization
	void Start () {
		Debug.Assert (MotionPath.Length + 1 == TargetVelocity.Length);
		currentMotionState = MOTION_STATE.AT_REST;
		originPath = new Vector3[MotionPath.Length + 1];
		originPath [0] = gameObject.transform.position;
		for (int i = 0; i < MotionPath.Length; ++i) {
			originPath [i + 1] = MotionPath [i];
		}

		constForce = gameObject.GetComponent<ConstantForce> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (DebugDraw) {
			for (int i = 1; i < originPath.Length; ++i) {
				Debug.DrawRay (originPath [i - 1], originPath [i], Color.blue);
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
			if (currentTargetPoint > originPath.Length) {
				currentTargetPoint = 0;
				currentMotionState = MOTION_STATE.AT_REST;
				constForce.force = Vector3.zero;
			} else {
				Debug.Log ("Moving to next target...");
				moveTowardsPoint (originPath [currentTargetPoint]);
			}
		}

		if (currentMotionState == MOTION_STATE.IN_MOTION) {
			float distanceUntilNextPoint = Vector3.Distance(gameObject.transform.position, originPath[currentTargetPoint]);
			Debug.Log (string.Format ("Logging distance until next target {0}", distanceUntilNextPoint));
		}
			
		if (DebugDraw) {
			Debug.DrawRay (gameObject.transform.position, debugDirection, Color.red);
		}


	}

	private void moveTowardsPoint(Vector3 p) {
		Vector3 diff = p - gameObject.transform.position;
		gameObject.GetComponent<Rigidbody>().AddForce(diff * gameObject.GetComponent<Rigidbody>().mass * TargetVelocity[currentTargetPoint]);
		if (DebugDraw) {
			debugDirection = diff;
		}
	}
}
