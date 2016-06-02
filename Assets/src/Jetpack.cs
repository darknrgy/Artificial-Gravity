using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {

    public float PowerScale;
    public SoundService SoundService;
    public MagneticShoes MagneticShoes;
    public InputService InputService;
    protected GameObject target;

    public void Apply(GameObject target) {

        this.target = target;

        float joystickAmplitude = 0.5f * PowerScale;
        float thrustAmplitude = 3.0f * PowerScale;
        
        if (Mathf.Abs(InputService.joyVerticalThrust) > 0.90f) {
            MagneticShoes.enabled = false;
        } else {
            MagneticShoes.enabled = true;
        }
   
        // Apply joystick input to torque
        var rigidbody = target.GetComponent<Rigidbody>();

        rigidbody.angularDrag = 0.0f;

        rigidbody.AddRelativeTorque(
            new Vector3(InputService.joyY * joystickAmplitude, 
            InputService.joyZ * joystickAmplitude / 2, 
            InputService.joyX * joystickAmplitude)
        );

        // Apply joystick input for thrut
        rigidbody.AddRelativeForce(
            new Vector3(InputService.joySideThrust * thrustAmplitude, 
            InputService.joyVerticalThrust * thrustAmplitude, 
            InputService.joyThrust * thrustAmplitude)
        );

        // Play Jetpack sounds
        float smoothing = 0.95f;

        if (InputService.joyVerticalThrust < 0) {
            SoundService.PlayAudioForJoystick(0, InputService.joyVerticalThrust, smoothing);
            SoundService.PlayAudioForJoystick(1, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(0, 0, smoothing);
            SoundService.PlayAudioForJoystick(1, InputService.joyVerticalThrust, smoothing);
        }

        if (InputService.joyThrust < 0) {
            SoundService.PlayAudioForJoystick(4, InputService.joyThrust, smoothing);
            SoundService.PlayAudioForJoystick(5, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(4, 0, smoothing);
            SoundService.PlayAudioForJoystick(5, InputService.joyThrust, smoothing);
        }

        if (InputService.joySideThrust < 0) {
            SoundService.PlayAudioForJoystick(2, InputService.joySideThrust, smoothing);
            SoundService.PlayAudioForJoystick(3, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(2, 0, smoothing);
            SoundService.PlayAudioForJoystick(3, InputService.joySideThrust, smoothing);
        }

        smoothing = 0.8f;
        SoundService.PlayAudioForJoystick(6, InputService.joyX, smoothing);
        SoundService.PlayAudioForJoystick(7, InputService.joyY, smoothing);
        SoundService.PlayAudioForJoystick(8, InputService.joyZ, smoothing);
    }
}
