using UnityEngine;
using System.Collections;

public class OrientationSelectableObjectCallback : SelectableObjectCallback {

    public GameObject OrentationReference;
    public bool DebugDraw;
    private bool rotatingTowards = false;
    private bool movingTowards = false;
    private Quaternion rotateTowards;

	// Use this for initialization
	void Start () {
        Debug.Assert(OrentationReference != null, "Please set a rotation reference object");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (DebugDraw)
        {
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 7.0f, Color.magenta);
        }

        if (rotatingTowards)
        {
            rotateTowards = Quaternion.LookRotation(OrentationReference.transform.position - gameObject.transform.position, OrentationReference.transform.up);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotateTowards, Time.deltaTime);
        }
        
	}

    override public bool ObjectSelected()
    {
        rotatingTowards = true;
        movingTowards = true;
        Debug.Log("Second Selected...");
        return true;
    }
}
