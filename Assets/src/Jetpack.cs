using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {

    public float PowerScale;
    public SoundService SoundService;
    public MagneticShoes MagneticShoes;
    public InputService InputService;

    protected new Rigidbody rigidbody;

    float audioSmoothing = 0.95f;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Apply() {
        rigidbody.angularDrag = 0f;
        rigidbody.drag = 0f;

        float joystickAmplitude = 0.5f * PowerScale;
        float thrustAmplitude = 3.0f * PowerScale;
        
        if (Mathf.Abs(InputService.MixedTrigger) > 0.90f) {
            MagneticShoes.enabled = false;
        } else {
            MagneticShoes.enabled = true;
        }
   
        // Apply joystick input to torque
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
        if (InputService.MixedTrigger < 0) {
            SoundService.PlayAudioForJoystick(0, InputService.MixedTrigger, audioSmoothing);
            SoundService.PlayAudioForJoystick(1, 0, audioSmoothing);
        } else {
            SoundService.PlayAudioForJoystick(0, 0, audioSmoothing);
            SoundService.PlayAudioForJoystick(1, InputService.MixedTrigger, audioSmoothing);
        }

        if (InputService.LeftStickY < 0) {
            SoundService.PlayAudioForJoystick(4, InputService.LeftStickY, audioSmoothing);
            SoundService.PlayAudioForJoystick(5, 0, audioSmoothing);
        } else {
            SoundService.PlayAudioForJoystick(4, 0, audioSmoothing);
            SoundService.PlayAudioForJoystick(5, InputService.LeftStickY, audioSmoothing);
        }

        if (InputService.MixedTriggerButtons < 0) {
            SoundService.PlayAudioForJoystick(2, InputService.MixedTriggerButtons, audioSmoothing);
            SoundService.PlayAudioForJoystick(3, 0, audioSmoothing);
        } else {
            SoundService.PlayAudioForJoystick(2, 0, audioSmoothing);
            SoundService.PlayAudioForJoystick(3, InputService.MixedTriggerButtons, audioSmoothing);
        }

        audioSmoothing = 0.8f;
        SoundService.PlayAudioForJoystick(6, InputService.RightStickX, audioSmoothing);
        SoundService.PlayAudioForJoystick(7, InputService.RightStickY, audioSmoothing);
        SoundService.PlayAudioForJoystick(8, InputService.LeftStickX, audioSmoothing);
    }

    public void PlayNonOpSounds() {
        for (int soundID = 0; soundID <= 8; soundID ++) {
            SoundService.PlayAudioForJoystick(soundID, 0, audioSmoothing);
        }
    }
}
