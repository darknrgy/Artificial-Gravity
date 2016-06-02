using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public OpenPhysicsObject physics;

    const int JETPACK_MODE = 1;
    const int GROUND_MODE = 2;
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

    // Add logic in these methods to transition
    protected void jetpackMode() {
        physics.SetJetpackEnabled(true);
        physics.SetFPSControllerEnabled(false);
    }

    protected void groundMode() {
        physics.SetJetpackEnabled(false);
        physics.SetFPSControllerEnabled(true);
    }
}
