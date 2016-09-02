using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject gameEnvironment;
    public GameObject playerXPrefab;
    public GameObject playerOPrefab;
    public GameObject emptyGridPrefab;
    public float gridDimension;
    public List<List<GameObject>> gridSquare;

    GameObject emptyGridParent, playerOXParent;
    bool playerXTurn;

    //Initialization
    void Start () {
        //Initialize Parent for instantiated prefabs
        emptyGridParent = new GameObject();
        emptyGridParent.name = "Empty Grid Parent";
        playerOXParent = new GameObject();
        playerOXParent.name = "Player OX Parent";

        //Generate background
        GameObject environment = Instantiate(gameEnvironment, this.transform.position, Quaternion.identity) as GameObject;
        environment.transform.SetParent(this.transform);

        //Generate grids
        gridSquare = new List<List<GameObject>>();
        List<GameObject> tempGridList = new List<GameObject>();
	    for (float i = 0; i < (gridDimension * 3); i = i + 3)
        {
            for (float j = 0; j < (gridDimension * 3); j = j + 3)
            {
                GameObject emptyGrid = Instantiate(emptyGridPrefab, new Vector3(i, j), Quaternion.identity) as GameObject;
                tempGridList.Add(emptyGrid); //add one grid to one column
                emptyGrid.transform.SetParent(emptyGridParent.transform); //group instantiated object to a parent
            }
            gridSquare.Add(tempGridList); //add one column of grid to a row
        }

        //Set turn
        playerXTurn = false;
	}
	
	// Update is called once per frame
	void Update () {

        // Left mouse button
	    if (Input.GetMouseButton(0))
        {
            if (playerXTurn) { InsertPlayerOX(playerXPrefab); } // Instantiate X
            else { Debug.Log("It's Player O Turn!"); }
        }

        // Right mouse button
        if (Input.GetMouseButton(1))
        {
            if (!playerXTurn) { InsertPlayerOX(playerOPrefab); } // Instantiate O
            else { Debug.Log("It's Player X Turn!"); }
        }
	}

    void InsertPlayerOX (GameObject playerOXPrefab)
    {
        // Get grid's position and instantiate player OX object
        RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (raycast.transform != null)
        {
            if (!raycast.transform.GetComponent<GridEvent>().getActiveBox())
            {
                raycast.transform.GetComponent<GridEvent>().setActiveBox(); //mark
                GameObject playerOX = Instantiate(playerOXPrefab, raycast.transform.position, Quaternion.identity) as GameObject;
                playerOX.transform.SetParent(playerOXParent.transform); //group instantiated object to a parent
                playerXTurn = !playerXTurn; //Switch the turn of the players
            }
        }
    }
}
