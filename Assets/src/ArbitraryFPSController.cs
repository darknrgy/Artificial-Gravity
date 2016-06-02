using UnityEngine;
using System.Collections;

public class ArbitraryFPSController : MonoBehaviour {

    public GameObject DownReference;
    public InputService InputService;

    const float forceMultiplier = 20.0f;

    // Use this for initialization
    protected PhysicsUtility physics;
	void Start () {
        physics = new PhysicsUtility();
	}
	
    public void Apply() {
        applyTorque();
        applyLinear();
    }

    protected void applyLinear() {
        // Apply joystick input for thrust
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 1.0f;
        rigidbody.AddRelativeForce(new Vector3(
                InputService.LeftStickX * forceMultiplier,
                0.0f,
                InputService.LeftStickY * forceMultiplier)
            );
    }

    protected float lastLocalYaw = 0;

    protected void applyTorque() {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 gravity = physics.GetGravityNormal(transform.position);
        Vector3 down = (transform.position - DownReference.transform.position).normalized;
        Vector3 diff = gravity - down;
        float magnitude = diff.magnitude;
        float angle = Vector3.Angle(down, gravity);
        Vector3 axisOfRotation = Vector3.Cross(gravity, down).normalized;

        // Below a minimum angle away from the target angle, just manually set the angle
        
        if (angle < 1.0f) {
            Debug.Log(angle);
            Quaternion r = Quaternion.AngleAxis(-angle, axisOfRotation);
            rigidbody.rotation = r * rigidbody.rotation;
            return;
        }
        float xComponent = Vector3.Dot(Vector3.left, axisOfRotation);
        float yComponent = Vector3.Dot(-Vector3.up, axisOfRotation);
        float zComponent = Vector3.Dot(-Vector3.forward, axisOfRotation);


        rigidbody.angularDrag = 10f;
        rigidbody.AddTorque(new Vector3(xComponent, yComponent, zComponent)* 100 * magnitude);        
    }
}
