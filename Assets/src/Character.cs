using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public OpenPhysicsObject Physics;
    public InputService InputService;

    const int JETPACK_MODE = 1;
    const int GROUND_MODE = 2;

    const float jetpackStartDelay = 1f; // seconds

    protected int mode = GROUND_MODE;

    public void Update() {
        switch (mode) {
        case JETPACK_MODE:
            jetpackMode();
            break;
        case GROUND_MODE:
            groundMode();
            break;
        }   
    }

    protected float enableGroundTimer = 0f;

    // Add logic in these methods to transition
    protected void jetpackMode() {
        Physics.FPSController.UpdateState();
        if (enableGroundTimer == 0f) {
            enableGroundTimer = Time.time;
        }
        if (enableGroundTimer > 1.0f) {

        }

        if (Physics.FPSController.hitDistance() < 3f) {
            mode = GROUND_MODE;
            return;
        }
        Physics.SetJetpackEnabled(true);
        Physics.SetFPSControllerEnabled(false);
    }

    protected float enableJetpackTimer = 0f;

    protected void groundMode() {
        if (Physics.FPSController.hitDistance() > 5f || !Physics.FPSController.groundDeteced()) {
            mode = JETPACK_MODE;
            return;
        }
        if (InputService.MixedTrigger > 0.5) {
            if (enableJetpackTimer == 0) {
                enableJetpackTimer = Time.time;
            }
            if (Time.time - enableJetpackTimer > jetpackStartDelay) {
                mode = JETPACK_MODE;
                return;
            }            
        } else {
            enableJetpackTimer = 0f;
        }
        Physics.SetJetpackEnabled(false);
        Physics.SetFPSControllerEnabled(true);
        Physics.Jetpack.PlayNonOpSounds();
    }
}
