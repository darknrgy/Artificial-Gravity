using UnityEngine;
using System.Collections;

public class SpinningSpaceStationPhysics : MonoBehaviour{
    
    public void Apply(GameObject target) {
        physics = new PhysicsUtility();
        this.target = target;
        ApplyLinear();
    }

    public void ApplyLinear() {
        Rigidbody tRigidBody = target.GetComponent<Rigidbody>();
        float angularVelocity = new Vector3(0, 0.05f, 0).magnitude;
        Vector3 tPosition = target.transform.position;
        Vector3 tVelocity= target.GetComponent<Rigidbody>().velocity;

        // Solve for the lateral force first

        // Get a unit vector in the direction of motion caused by spin
        Vector3 fNormalized = physics.GetForceVectorNormalized(tPosition);

        // Get the radii of the current position and the next position
        float radius = physics.GetDeltaVector(physics.GetClosestPointOnAxis(tPosition), tPosition).magnitude;
        float nextRadius = physics.GetDeltaVector(physics.GetClosestPointOnAxis(tPosition + tVelocity), tPosition + tVelocity).magnitude;

        // Get the difference in the two linear velocities caused by angular velocity
        float linearVelocityDelta = physics.GetLinearVelocity(angularVelocity, nextRadius) - physics.GetLinearVelocity(angularVelocity, radius);

        // Build a force vector in the directino of the unit vector with magnitude of linear velocity delta
        Vector3 f = fNormalized * linearVelocityDelta;

        // Add this to velocity
        tRigidBody.velocity += f * Time.fixedDeltaTime;

        // Now solve for centrifugal force

        // Obtain the fNormalized component of the current velocity
        float fComponent = Vector3.Dot(fNormalized, tVelocity);

        // Add the space station spin
        float fComponentSpaceStation = physics.GetLinearVelocity(angularVelocity, radius);

        // Get centrifugal force
        float centrifugalForce = Mathf.Pow(fComponent - fComponentSpaceStation, 2) / radius;

        // Get the unit vector for the direction of centrifugal force
        Vector3 centrifugalForceVector = (tPosition - physics.GetClosestPointOnAxis(tPosition)).normalized;

        // Add centrifugal force to velocity
        tRigidBody.velocity += centrifugalForceVector * centrifugalForce * Time.fixedDeltaTime;


        // Now solve for anti-centrifugal force
        // Regardless of direction you are always being pulled towards the center when you move
        // TODO: This works but is pretty obviously not correct and requires a mystery coefficient 
        float antiCentrifugalForce = Mathf.Pow(fComponent, 2) / radius;
        if (antiCentrifugalForce > 1.8f) antiCentrifugalForce = 1.8f;
        tRigidBody.velocity -= centrifugalForceVector * antiCentrifugalForce * Time.fixedDeltaTime;
    }

    protected GameObject target;
    protected PhysicsUtility physics;
}
