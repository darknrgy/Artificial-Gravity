using UnityEngine;
using System.Collections;

public class SelectableGameObject : MonoBehaviour {

    /* 
     * 
     */
    public GameObject Selector;
    public float Distance;
    public bool DebugDraw;
    private SelectableObjectCallback callbackObject;
    private bool outlineDirty = false;

    // Use this for initialization
    void Start () {
        SelectableObjectCallback callbackComponent = gameObject.GetComponent<SelectableObjectCallback>();
        callbackObject = callbackComponent;

        Debug.Assert(Selector != null, "Please assign a selector object");
        Debug.Assert(callbackComponent != null, "Please assign a callback to this object, if you are going to make it selectable");
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forwardRay = Selector.transform.forward.normalized * Distance;
        RaycastHit outInfo;

        if (DebugDraw)
        {
            Debug.DrawRay(Selector.transform.position, forwardRay, Color.cyan);
        }

        if (Physics.Raycast(Selector.transform.position, forwardRay, out outInfo, 10.0f))
        {
            setThickness(1.1f);
            outlineDirty = true;
        }
        else
        {
            if (outlineDirty)
            {
                setThickness(1.0f);
                outlineDirty = false;
            }
        }
	}

    private void setThickness(float thickness)
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_Thickness", thickness);
    }
}
