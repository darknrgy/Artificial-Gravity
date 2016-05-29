using UnityEngine;
using System.Collections;

public class PhysicsUtility{

    // Convert angular velocity and radians to linear velocity
    public float GetLinearVelocity(float angularVelocity, float radius) {
        return angularVelocity * radius;
    }

    // Get tangent to axis
    public Vector3 GetForceVectorNormalized(Vector3 target) {
        Vector3 axisHelper = GetClosestPointOnAxis(target);
        Vector3 forceVector = Vector3.Cross(target, axisHelper).normalized;

        // Cross product reverses direction at y = 0
        if (axisHelper.y > 0) {
            forceVector *= -1;
        }

        return forceVector;
    }

    // Calcululate the closest point to the target that is on the axis line
    public Vector3 GetClosestPointOnAxis(Vector3 target) {
        return new Vector3(0, target.y, 0);
    }

    // Get the difference between two vectors
    public Vector3 GetDeltaVector(Vector3 a, Vector3 b) {
        return b - a;
    }
}
