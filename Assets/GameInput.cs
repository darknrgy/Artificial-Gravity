using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour {

    public float joyX { get; private set; }
    public float joyY { get; private set; }
    public float joyZ { get; private set; }

    public float joyThrustFwd { get; private set; }
    public float joyThrustRev { get; private set; }
    public float joyThrustUp { get; private set; }
    public float joyThrustDown { get; private set; }
    public float joyVerticalThrust { get; private set; }
    public float joyThrust { get; private set; }
    public float joySideThrust { get; private set; }

    void Start () {
        CalibrateJoystick();
    }
	
	void FixedUpdate () {

        if (Input.GetButton("Center Joystick")) {
            CalibrateJoystick();
        }

        // Collect joystick input
        joyX = Input.GetAxis("Roll") - xCal;
        joyY = Input.GetAxis("Pitch") - yCal;
        joyZ = Input.GetAxis("Yaw") - zCal;

        //float joyVerticalThrust = Input.GetAxis("Thrust Vertical") - tvCal;
        joyThrustFwd = Input.GetAxis("Thrust Fwd") - tFwdCal;
        joyThrustRev = Input.GetAxis("Thrust Rev") - tRevCal;
        joyThrustUp = Input.GetAxis("Thrust Up");
        joyThrustDown = Input.GetAxis("Thrust Down");
        joyVerticalThrust = (joyThrustUp - joyThrustDown) / 2;

        joyThrust = Input.GetAxis("Thrust") - tCal;
        joySideThrust = 0;

        // Linear thrust keyboard input
        joySideThrust =     applyButtonToAxis(joySideThrust, "Thrust Left", -1);
        joySideThrust =     applyButtonToAxis(joySideThrust, "Thrust Right", 1);
        joyVerticalThrust = applyButtonToAxis(joyVerticalThrust, "Thrust Up", 1);
        joyVerticalThrust = applyButtonToAxis(joyVerticalThrust, "Thrust Down", -1);
        joyThrust =         applyButtonToAxis(joyThrust, "Thrust Fwd", 1);
        joyThrust =         applyButtonToAxis(joyThrust, "Thrust Rev", -1);

        // Attitude keyboard input
        joyX =              applyButtonToAxis(joyX, "Roll Right", -1);
        joyX =              applyButtonToAxis(joyX, "Roll Left", 1);
        joyY =              applyButtonToAxis(joyY, "Pitch Down", 1);
        joyY =              applyButtonToAxis(joyY, "Pitch Up", -1);
        joyZ =              applyButtonToAxis(joyZ, "Yaw Left", -1);
        joyZ =              applyButtonToAxis(joyZ, "Yaw Right", 1);
    }

    protected float xCal = 0;
    protected float yCal = 0;
    protected float zCal = 0;
    protected float tvCal = 0;
    protected float tCal = 0;
    protected float tFwdCal = 0;
    protected float tRevCal = 0;

    void CalibrateJoystick() {
        xCal = Input.GetAxis("Roll");
        yCal = Input.GetAxis("Pitch");
        zCal = Input.GetAxis("Yaw");
        tvCal = Input.GetAxis("Thrust Vertical");
        tCal = Input.GetAxis("Thrust");
    }

    protected float applyButtonToAxis(float axis, string inputName, float value) {
        if (Input.GetButton(inputName)) {
            axis = value;
        }
        return axis;
    }
}

