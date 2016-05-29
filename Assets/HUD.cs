using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    public Rigidbody MyRigidBody;
    public TextMesh Velocity;
    public TextMesh Altitude;
    
    // Use this for initialization
	void Start () {
	
	}

    public void SetVelocity(float velocity) {
        Velocity.text = "VEL " + velocity.ToString("F1");
    }

    public void SetAltitude(float altitude) {
        Altitude.text = "ALT " + altitude.ToString("F1");
    }
	
	// Update is called once per frame
	void Update () {

    }
}
