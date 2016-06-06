using UnityEngine;
using System.Collections;

public class SelectableObjectCallback : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public bool ObjectSelected()
    {
        Debug.Log("Hello Selected....");
        return false;
    }
}
