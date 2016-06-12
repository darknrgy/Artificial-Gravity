using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public GameObject CharacterGameObject;

    public Text Velocity;
    public Text Altitude;
    
    void Update () {
        SetVelocity(CharacterGameObject.GetComponent<Rigidbody>().velocity.magnitude);
        SetAltitude(926f - CharacterGameObject.transform.position.magnitude);
    }

    public void SetVelocity(float velocity) {
        Velocity.text = "VEL " + velocity.ToString("F1");
    }

    public void SetAltitude(float altitude) {
        Altitude.text = "ALT " + altitude.ToString("F1");
    }
}
