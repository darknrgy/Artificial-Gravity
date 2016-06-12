using UnityEngine;
using System.Collections;

public class ArbitraryFPSController : MonoBehaviour {

    public GameObject DownReference;
    public InputService InputService;
    public CapsuleCollider CapsuleCollider;

    protected bool isGrounded = false;
    protected new Rigidbody rigidbody;
    protected Vector3 gravity;
    protected Vector3 down;
    protected Vector3 diff;
    protected float magnitude;
    protected float angle;
    protected Vector3 axisOfRotation;
    protected bool isJumping { get; private set; }
    protected bool isHit { get; private set; }
    RaycastHit hitInfo;

    const int STATE_NONE =              1;
    const int STATE_GRAVITY =           2;
    const int STATE_MAGNETICBOOTS =     3;
    const float forceMultiplier =       20.0f;

    // Use this for initialization
    protected PhysicsUtility physics;
	void Start () {
        physics = new PhysicsUtility();
        rigidbody = GetComponent<Rigidbody>();
        isJumping = false;
    }
	
    public void Apply() {
        rigidbody.angularDrag = 10f;
        rigidbody.drag = 1.0f;
        UpdateState();
        applyJoystickToLinear();
        applyJoystickToRotation();
        if (!isGrounded || isJumping) return;
        alignVelocityToGround();
        setHeight();
        AlignOrientation();
    }

    public void AlignOrientation() {
        // Below a minimum angle away from the target angle, just manually set the angle        
        if (angle < 1.0f) {
            Quaternion r = Quaternion.AngleAxis(-angle, axisOfRotation);
            rigidbody.rotation = r * rigidbody.rotation;
            return;
        }

        float xComponent = Vector3.Dot(Vector3.left, axisOfRotation);
        float yComponent = Vector3.Dot(-Vector3.up, axisOfRotation);
        float zComponent = Vector3.Dot(-Vector3.forward, axisOfRotation);
        
        rigidbody.AddTorque(new Vector3(xComponent, yComponent, zComponent) * 50 * magnitude);
    }

    protected void applyJoystickToLinear() {
        // Apply joystick input for thrust
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (InputService.MixedTrigger > 0.5) {
            if (isGrounded && !isJumping) {
                rigidbody.velocity -= (down * 5);
                isJumping = true;
            }
        }

        rigidbody.AddRelativeForce(new Vector3(
            InputService.LeftStickX * forceMultiplier,
            0,
            InputService.LeftStickY * forceMultiplier)
        );  
    }

    protected void applyJoystickToRotation() {
        rigidbody.AddTorque(InputService.RightStickX * -5f, 0, 0);
    }

    protected void findGround() {
        int layers = ~0;

        isHit = Physics.SphereCast(
            transform.position, CapsuleCollider.radius,
            down,
            out hitInfo,
            10f,
            layers,
            QueryTriggerInteraction.Ignore
        );

        if (!isHit) {
            isGrounded = false;
            return;
        }

        isGrounded = hitInfo.distance < 2.5f;
        if (isJumping && hitInfo.distance > 2.5f) {
            isJumping = false;
        }
    }

    public bool groundDeteced() {
        return isHit;
    }

    public float hitDistance() {
        return hitInfo.distance;
    }

    protected void alignVelocityToGround() {
        rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, hitInfo.normal);
    }

    protected void setHeight() {
        if (!isGrounded) return;
        float distance = 2.5f - hitInfo.distance;
        Vector3 up = -down;
        rigidbody.position += up * distance;
    }

    public void UpdateState() {
        gravity = physics.GetGravityNormal(transform.position);
        down = (DownReference.transform.position - transform.position).normalized;
        diff = gravity - down;
        magnitude = diff.magnitude;
        angle = Vector3.Angle(down, gravity);
        axisOfRotation = Vector3.Cross(gravity, down).normalized;
        findGround();
    }
}
