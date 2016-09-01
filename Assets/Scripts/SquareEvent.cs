using UnityEngine;
using System.Collections;

public class SquareEvent : MonoBehaviour {

    private bool activeBox;
	// Use this for initialization
	void Start () {
        activeBox = false;
	}
	
	// Update is called once per frame
	public void setActiveBox()
    {
        activeBox = true;
    }

    public bool getActiveBox()
    {
        return activeBox;
    }
}
