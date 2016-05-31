using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {

    public float PowerScale;
    public SoundService SoundService;
    public MagneticShoes MagneticShoes;
    public GameInput input;
    protected GameObject target;

    public void Apply(GameObject target) {

        this.target = target;

        float joystickAmplitude = 0.5f * PowerScale;
        float thrustAmplitude = 8.0f * PowerScale;
        
        if (Mathf.Abs(input.joyVerticalThrust) > 0.90f) {
            MagneticShoes.enabled = false;
        } else {
            MagneticShoes.enabled = true;
        }
   
        // Apply joystick input to torque
        var rigidbody = target.GetComponent<Rigidbody>();

        rigidbody.angularDrag = 0.0f;

        rigidbody.AddRelativeTorque(new Vector3(input.joyY * joystickAmplitude, input.joyZ * joystickAmplitude / 2, input.joyX * joystickAmplitude));

        // Apply joystick input for thrut
        rigidbody.AddRelativeForce(new Vector3(input.joySideThrust * thrustAmplitude, input.joyVerticalThrust * thrustAmplitude, input.joyThrust * thrustAmplitude));

        // Play Jetpack sounds
        float smoothing = 0.95f;

        if (input.joyVerticalThrust < 0) {
            SoundService.PlayAudioForJoystick(0, input.joyVerticalThrust, smoothing);
            SoundService.PlayAudioForJoystick(1, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(0, 0, smoothing);
            SoundService.PlayAudioForJoystick(1, input.joyVerticalThrust, smoothing);
        }

        if (input.joyThrust < 0) {
            SoundService.PlayAudioForJoystick(4, input.joyThrust, smoothing);
            SoundService.PlayAudioForJoystick(5, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(4, 0, smoothing);
            SoundService.PlayAudioForJoystick(5, input.joyThrust, smoothing);
        }

        if (input.joySideThrust < 0) {
            SoundService.PlayAudioForJoystick(2, input.joySideThrust, smoothing);
            SoundService.PlayAudioForJoystick(3, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(2, 0, smoothing);
            SoundService.PlayAudioForJoystick(3, input.joySideThrust, smoothing);
        }

        smoothing = 0.8f;
        SoundService.PlayAudioForJoystick(6, input.joyX, smoothing);
        SoundService.PlayAudioForJoystick(7, input.joyY, smoothing);
        SoundService.PlayAudioForJoystick(8, input.joyZ, smoothing);
    }
}
