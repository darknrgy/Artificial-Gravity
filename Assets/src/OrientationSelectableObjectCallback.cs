using UnityEngine;
using System.Collections;

public class OrientationSelectableObjectCallback : SelectableObjectCallback {

    public GameObject OrentationReference;
    private bool rotatingTowards = false;
    private Quaternion rotateTowards;

	// Use this for initialization
	void Start () {
        Debug.Assert(OrentationReference != null, "Please set a rotation reference object");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (rotatingTowards)
        {
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotateTowards, Time.deltaTime);
        }
        
	}

    override public bool ObjectSelected()
    {
        rotatingTowards = true;
        rotateTowards = Quaternion.LookRotation(OrentationReference.transform.position);
        Debug.Log("Second Selected...");
        return true;
    }
}
