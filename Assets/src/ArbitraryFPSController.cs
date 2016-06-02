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
}
