using UnityEngine;
using System.Collections;

public class SpinningSpaceStationPhysics : MonoBehaviour{
    
    public void Apply(GameObject target) {
        this.target = target;
        ApplyLinear();
    }

    public void ApplyLinear() {
        Rigidbody tRigidBody = target.GetComponent<Rigidbody>();
        float mass = tRigidBody.mass;
        float angularVelocity = new Vector3(0, 0.05f, 0).magnitude;
        Vector3 tPosition = target.transform.position;
        Vector3 tVelocity= target.GetComponent<Rigidbody>().velocity;

        // Solve for the lateral force first

        // Get a unit vector in the direction of motion caused by spin
        Vector3 fNormalized = GetForceVectorNormalized(tPosition);

        // Get the radii of the current position and the next position
        float radius = GetDeltaVector(GetClosestPointOnAxis(tPosition), tPosition).magnitude;
        float nextRadius = GetDeltaVector(GetClosestPointOnAxis(tPosition + tVelocity), tPosition + tVelocity).magnitude;

        // Get the difference in the two linear velocities caused by angular velocity
        float linearVelocityDelta = GetLinearVelocity(angularVelocity, nextRadius) - GetLinearVelocity(angularVelocity, radius);

        // Build a force vector in the directino of the unit vector with magnitude of linear velocity delta
        Vector3 f = fNormalized * linearVelocityDelta;

        // Add this to velocity
        tRigidBody.velocity += f * Time.fixedDeltaTime * mass;

        // Now solve for centrifugal force

        // Obtain the fNormalized component of the current velocity
        float fComponent = Vector3.Dot(fNormalized, tVelocity);

        // Add the space station spin
        float fComponentSpaceStation = GetLinearVelocity(angularVelocity, radius);

        // Get centrifugal force
        float centrifugalForce = Mathf.Pow(fComponent - fComponentSpaceStation, 2) / radius;

        // Get the unit vector for the direction of centrifugal force
        Vector3 centrifugalForceVector = (tPosition - GetClosestPointOnAxis(tPosition)).normalized;

        // Add centrifugal force to velocity
        tRigidBody.velocity += centrifugalForceVector * centrifugalForce * Time.fixedDeltaTime * mass;


        // Now solve for anti-centrifugal force
        // Regardless of direction you are always being pulled towards the center when you move
        // TODO: This works but is pretty obviously not correct and requires a mystery coefficient 
        float antiCentrifugalForce = Mathf.Pow(fComponent, 2) / radius;
        if (antiCentrifugalForce > 1.8f) antiCentrifugalForce = 1.8f;
        tRigidBody.velocity -= centrifugalForceVector * antiCentrifugalForce * Time.fixedDeltaTime;
    }

    // Get the vector of the moving air
    protected Vector3 GetForceVectorNormalized(Vector3 target) {
        Vector3 axisHelper = GetClosestPointOnAxis(target);
        Vector3 forceVector = Vector3.Cross(target, axisHelper).normalized;

        // Cross product reverses direction at y = 0
        if (axisHelper.y > 0) {
            forceVector *= -1;
        }

        return forceVector;
    }

    // Calcululate the closest point to the target that is on the axis line
    protected Vector3 GetClosestPointOnAxis(Vector3 target) {
        return new Vector3(0, target.y, 0);
    }

    // Get the difference between two vectors
    protected Vector3 GetDeltaVector(Vector3 a, Vector3 b) {
        return b - a;
    }

    // Convert angular velocity and radians to linear velocity
    protected float GetLinearVelocity(float angularVelocity, float radius) {
        return angularVelocity * radius;
    }

    protected GameObject target;
}
