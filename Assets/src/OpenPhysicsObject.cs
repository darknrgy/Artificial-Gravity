using UnityEngine;
using System;
using System.Collections;

/* 
 * This is the main class for physics acting on an untethered object
 */

public class OpenPhysicsObject : MonoBehaviour {

    public SpinningSpaceStationPhysics SpinningSpaceStationPhysics;
    public AirResistance AirResistance;
    public Jetpack Jetpack;
    public HUD hud;

    public void Start() {
    }

    void Update() {
        hud.SetVelocity(GetComponent<Rigidbody>().velocity.magnitude);
        hud.SetAltitude(926f - transform.position.magnitude);
    }

    void FixedUpdate() {
        var forceComponent = GetComponent<ConstantForce>();
        forceComponent.force = Vector3.zero;
        forceComponent.relativeForce = Vector3.zero;

        AirResistance.Apply(gameObject);
        SpinningSpaceStationPhysics.Apply(gameObject);
        Jetpack.Apply(gameObject);        
    }
}
