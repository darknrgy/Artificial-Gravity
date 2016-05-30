using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {

    public float PowerScale;
    public SoundService SoundService;
    public MagneticShoes MagneticShoes;

    public void Apply(GameObject target) {
        this.target = target;

        float joystickAmplitude = 0.5f * PowerScale;
        float thrustAmplitude = 8.0f * PowerScale;

        // Collect joystick input
        float joyX = Input.GetAxis("Roll") - xCal;
        float joyY = Input.GetAxis("Pitch") - yCal;
        float joyZ = Input.GetAxis("Yaw") - zCal;
        //float joyVerticalThrust = Input.GetAxis("Thrust Vertical") - tvCal;
        float joyThrustFwd = Input.GetAxis("Thrust Fwd") - tFwdCal;
        float joyThrustRev = Input.GetAxis("Thrust Rev") - tRevCal;
        float joyThrustUp = Input.GetAxis("Thrust Up");
        float joyThrustDown = Input.GetAxis("Thrust Down");
        float joyVerticalThrust = (joyThrustUp - joyThrustDown) / 2;

        float joyThrust = Input.GetAxis("Thrust") - tCal;
        float joySideThrust = 0;

        // Linear thrust keyboard input
        applyButtonToAxis(ref joySideThrust, "Thrust Left", -1);
        applyButtonToAxis(ref joySideThrust, "Thrust Right", 1);
        applyButtonToAxis(ref joyVerticalThrust, "Thrust Up", 1);
        applyButtonToAxis(ref joyVerticalThrust, "Thrust Down", -1);
        applyButtonToAxis(ref joyThrust, "Thrust Fwd", 1);
        applyButtonToAxis(ref joyThrust, "Thrust Rev", -1);

        // Attitude keyboard input
        applyButtonToAxis(ref joyX, "Roll Right", -1);
        applyButtonToAxis(ref joyX, "Roll Left", 1);
        applyButtonToAxis(ref joyY, "Pitch Down", 1);
        applyButtonToAxis(ref joyY, "Pitch Up", -1);
        applyButtonToAxis(ref joyZ, "Yaw Left", -1);
        applyButtonToAxis(ref joyZ, "Yaw Right", 1);

        if (Mathf.Abs(joyVerticalThrust) > 0.90f) {
            MagneticShoes.enabled = false;
        } else {
            MagneticShoes.enabled = true;
        }
   
        if (Input.GetButton("Center Joystick")) {
            CalibrateJoystick();
        }

        // Apply joystick input to torque
        var rigidbody = target.GetComponent<Rigidbody>();
        rigidbody.AddRelativeTorque(new Vector3(joyY * joystickAmplitude, joyZ * joystickAmplitude / 2, joyX * joystickAmplitude));

        // Apply joystick input for thrut
        rigidbody.AddRelativeForce(new Vector3(joySideThrust * thrustAmplitude, joyVerticalThrust * thrustAmplitude, joyThrust * thrustAmplitude));

        // Play Jetpack sounds
        float smoothing = 0.95f;

        if (joyVerticalThrust < 0) {
            SoundService.PlayAudioForJoystick(0, joyVerticalThrust, smoothing);
            SoundService.PlayAudioForJoystick(1, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(0, 0, smoothing);
            SoundService.PlayAudioForJoystick(1, joyVerticalThrust, smoothing);
        }

        if (joyThrust < 0) {
            SoundService.PlayAudioForJoystick(4, joyThrust, smoothing);
            SoundService.PlayAudioForJoystick(5, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(4, 0, smoothing);
            SoundService.PlayAudioForJoystick(5, joyThrust, smoothing);
        }


        if (joySideThrust < 0) {
            SoundService.PlayAudioForJoystick(2, joySideThrust, smoothing);
            SoundService.PlayAudioForJoystick(3, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(2, 0, smoothing);
            SoundService.PlayAudioForJoystick(3, joySideThrust, smoothing);
        }

        smoothing = 0.8f;
        SoundService.PlayAudioForJoystick(6, joyX, smoothing);
        SoundService.PlayAudioForJoystick(7, joyY, smoothing);
        SoundService.PlayAudioForJoystick(8, joyZ, smoothing);
    }

    protected void applyButtonToAxis(ref float axis, string inputName, float value) {
        if (Input.GetButton(inputName)) {
            axis = value;
        }
        
    }

    void CalibrateJoystick() {
        xCal = Input.GetAxis("Roll");
        yCal = Input.GetAxis("Pitch");
        zCal = Input.GetAxis("Yaw");
        tvCal = Input.GetAxis("Thrust Vertical");
        tCal = Input.GetAxis("Thrust");
    }

    protected GameObject target;

    protected float xCal = 0;
    protected float yCal = 0;
    protected float zCal = 0;
    protected float tvCal = 0;
    protected float tCal = 0;
    protected float tFwdCal = 0;
    protected float tRevCal = 0;
}
