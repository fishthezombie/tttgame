using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject tttObject;
    public GameObject squareCollider;

    // Use this for initialization
    void Start () {
	    for (float i = -3; i <= 3; i = i + 3)
        {
            for (float j = -3; j <= 3; j = j + 3)
            {
                GameObject square = Instantiate(squareCollider, new Vector3(i, j), Quaternion.identity) as GameObject;
                //square.transform.SetParent(squares.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(0))
        {
            RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycast.transform != null)
            {
                if (!raycast.transform.GetComponent<SquareEvent>().getActiveBox())
                {
                    raycast.transform.GetComponent<SquareEvent>().setActiveBox();
                    GameObject insertObject = Instantiate(tttObject, raycast.transform.position, Quaternion.identity) as GameObject;
                }
            }
        }
	}
}
