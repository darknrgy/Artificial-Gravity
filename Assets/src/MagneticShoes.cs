using UnityEngine;
using System.Collections;

public class MagneticShoes : MonoBehaviour {

    public float InitalStrength;
    public float DistanceBeforeShoesTakeEffect;
    public bool DebugDraw;
    public GameObject Magnet;

    private float CurrentStrength;

    // Use this for initialization
    void Start() {
        CurrentStrength = InitalStrength;
    }

    void FixedUpdate() {
        Vector3 magnetPosition = Magnet.transform.position;
        if (Input.GetKeyUp(KeyCode.Comma)) {
            CurrentStrength += 1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.Period)) {
            CurrentStrength -= 1.0f;
        }

        Vector3 dir = -gameObject.transform.up;
        RaycastHit hit = new RaycastHit();

        if (DebugDraw) {
            // Draw a line out of the magnet, straight down relative to the player's orientation
            Debug.DrawRay(magnetPosition, dir * DistanceBeforeShoesTakeEffect, Color.green);
        }

        // Cast a ray out of the players feet for the distance specified by (DistanceBeforeShoesTakeEffect)
        if (Physics.Raycast(magnetPosition, dir, out hit, DistanceBeforeShoesTakeEffect)) {
            // We have contact perform calculations to determine the inverse distance squared
            float inverseDistanceSquared = 1.0f / Mathf.Max(hit.distance * hit.distance, 0.00001f); // Avoid division by 0

            // Apply the force downward from the magnet object in the direction along the normal of the contacted object
            Magnet.GetComponent<Rigidbody>().AddForce(-hit.normal * CurrentStrength * inverseDistanceSquared);

            if (DebugDraw) {
                // Draw a line normal from the point that is hit
                Debug.DrawRay(hit.point, hit.normal * DistanceBeforeShoesTakeEffect, Color.red);

                // Draw a line from the magnet to the normal of the surface
                Debug.DrawRay(magnetPosition, -hit.normal * hit.distance, Color.yellow);
            }
        }
    }
}
