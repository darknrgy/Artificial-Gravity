using UnityEngine;
using System.Collections;

public class ArbitraryFPSController : MonoBehaviour {

    public GameObject DownReference;

    // Use this for initialization
    protected PhysicsUtility physics;
	void Start () {
        physics = new PhysicsUtility();
	}
	
    public void Apply() {
        applyTorque();
        applyLinear();

        if (Input.GetButton("Center Joystick")) {
            CalibrateJoystick();
        }
    }

    protected void applyLinear() {

    }

    protected void applyTorque() {
        Vector3 gravity = physics.GetGravityNormal(transform.position);
        Vector3 down = (transform.position - DownReference.transform.position).normalized;
        Vector3 diff = gravity - down;
        float magnitude = diff.magnitude;

        Vector3 axisOfRotation = Vector3.Cross(gravity, down).normalized;

        Debug.DrawRay(transform.position, down, Color.red);
        Debug.DrawRay(transform.position, gravity, Color.green);

        float xComponent = Vector3.Dot(Vector3.left, axisOfRotation);
        float yComponent = Vector3.Dot(-Vector3.up, axisOfRotation);
        float zComponent = Vector3.Dot(-Vector3.forward, axisOfRotation);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularDrag = 10f;
        rigidbody.AddTorque(new Vector3(xComponent, yComponent, zComponent)* 100 * magnitude);        
    }

    protected float xCal = 0;
    protected float yCal = 0;
    protected float zCal = 0;
    protected float tvCal = 0;
    protected float tCal = 0;
    protected float tFwdCal = 0;
    protected float tRevCal = 0;

    void CalibrateJoystick() {
        xCal = Input.GetAxis("Roll");
        yCal = Input.GetAxis("Pitch");
        zCal = Input.GetAxis("Yaw");
        tvCal = Input.GetAxis("Thrust Vertical");
        tCal = Input.GetAxis("Thrust");
    }

}
