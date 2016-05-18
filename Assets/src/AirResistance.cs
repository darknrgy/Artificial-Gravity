using UnityEngine;
using System;
using System.Collections;


public class AirResistance : MonoBehaviour {

    public SoundService SoundService;

    public void Apply(GameObject target) {
        this.target = target;
        ApplyLinear();
        ApplyTorque();
    }

    protected void ApplyLinear() {
        // Setup variables
        float angularVelocity = 0;
        Vector3 targetVector = target.transform.position;
        Vector3 targetVelocityVector = target.GetComponent<Rigidbody>().velocity;
        ConstantForce targetForce = target.GetComponent<ConstantForce>();
        
        // Get air vector
        Vector3 forceVectorNormalized = GetForceVectorNormalized(targetVector);
        float radius = GetDeltaVector(GetClosestPointOnAxis(targetVector), targetVector).magnitude;
        float forceMagnitude = GetLinearVelocity(angularVelocity, radius);
        Vector3 forceVector = forceVectorNormalized * forceMagnitude;

        // Apply air vector to target
        Vector3 frictionVelocityVector = forceVector - targetVelocityVector;
        targetForce.force += frictionVelocityVector.normalized * (float)Math.Pow(frictionVelocityVector.magnitude, 2) * 0.005f;

        // Make wind sound
        float airVelocityNomalized = frictionVelocityVector.magnitude / 40f;
        if (airVelocityNomalized > 1.0f) airVelocityNomalized = 1.0f;
        if (airVelocityNomalized < 0.0f) airVelocityNomalized = 0.0f;
        airVelocityNomalized = (float) Math.Pow(airVelocityNomalized, 2.0f);

        SoundService.PlayAudioForValue(9, airVelocityNomalized, 0.5f, 0, 1.0f, 0.9f, 1.5f, 200f, 6000f);
    }

    protected void ApplyTorque() {
        Vector3 targetVector = target.transform.position;
        var rigidbody = target.GetComponent<Rigidbody>();
        float radius = GetDeltaVector(GetClosestPointOnAxis(targetVector), targetVector).magnitude;
        if (radius > 600) radius = 600.0f;
        Vector3 realAngularVelocity = - new Vector3(0, 0.05f, 0) * (1 - (radius / 600));
        //Vector3 realAngularVelocity = Vector3.zero;
        Vector3 angularVelocityDelta = realAngularVelocity - target.GetComponent<Rigidbody>().angularVelocity;
        rigidbody.AddTorque(angularVelocityDelta * Time.fixedDeltaTime * 10 * (radius / 600)) ;
    }

    // Convert angular velocity and radians to linear velocity
    protected float GetLinearVelocity(float angularVelocity, float radius) {
        return angularVelocity * radius;
    }


    // Calcululate the closest point to the target that is on the axis line
    protected Vector3 GetClosestPointOnAxis(Vector3 target) {
        return new Vector3(0, target.y, 0);
    }

    // Get the difference between two vectors
    protected Vector3 GetDeltaVector(Vector3 a, Vector3 b) {
        return b - a;
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

    protected GameObject target;
    protected SoundService sound;
}
