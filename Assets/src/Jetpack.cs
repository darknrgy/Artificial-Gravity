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
        
        if (Mathf.Abs(InputService.MixedTrigger) > 0.90f) {
            MagneticShoes.enabled = false;
        } else {
            MagneticShoes.enabled = true;
        }
   
        // Apply joystick input to torque
        var rigidbody = target.GetComponent<Rigidbody>();

        rigidbody.angularDrag = 0.0f;

        rigidbody.AddRelativeTorque(
            new Vector3(InputService.RightStickY * joystickAmplitude, 
            InputService.LeftStickX * joystickAmplitude / 2, 
            InputService.RightStickX * joystickAmplitude)
        );

        // Apply joystick input for thrust
        rigidbody.AddRelativeForce(
            new Vector3(InputService.MixedTriggerButtons * thrustAmplitude, 
            InputService.MixedTrigger * thrustAmplitude, 
            InputService.LeftStickY * thrustAmplitude)
        );

        // Play Jetpack sounds
        float smoothing = 0.95f;

        if (InputService.MixedTrigger < 0) {
            SoundService.PlayAudioForJoystick(0, InputService.MixedTrigger, smoothing);
            SoundService.PlayAudioForJoystick(1, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(0, 0, smoothing);
            SoundService.PlayAudioForJoystick(1, InputService.MixedTrigger, smoothing);
        }

        if (InputService.LeftStickY < 0) {
            SoundService.PlayAudioForJoystick(4, InputService.LeftStickY, smoothing);
            SoundService.PlayAudioForJoystick(5, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(4, 0, smoothing);
            SoundService.PlayAudioForJoystick(5, InputService.LeftStickY, smoothing);
        }

        if (InputService.MixedTriggerButtons < 0) {
            SoundService.PlayAudioForJoystick(2, InputService.MixedTriggerButtons, smoothing);
            SoundService.PlayAudioForJoystick(3, 0, smoothing);
        } else {
            SoundService.PlayAudioForJoystick(2, 0, smoothing);
            SoundService.PlayAudioForJoystick(3, InputService.MixedTriggerButtons, smoothing);
        }

        smoothing = 0.8f;
        SoundService.PlayAudioForJoystick(6, InputService.RightStickX, smoothing);
        SoundService.PlayAudioForJoystick(7, InputService.RightStickY, smoothing);
        SoundService.PlayAudioForJoystick(8, InputService.LeftStickX, smoothing);
    }
}
