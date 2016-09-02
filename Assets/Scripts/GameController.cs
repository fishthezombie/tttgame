using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject gameEnvironment;
    public GameObject playerXPrefab;
    public GameObject playerOPrefab;
    public GameObject emptyGridPrefab;
    public float gridDimension;
    public List<List<GameObject>> gridSquareList;

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
        gridSquareList = new List<List<GameObject>>();
        List<GameObject> tempGridList;
	    for (float i = 0; i < (gridDimension * 3); i = i + 3) {
            tempGridList = new List<GameObject>();
            for (float j = 0; j < (gridDimension * 3); j = j + 3) {
                GameObject emptyGrid = Instantiate(emptyGridPrefab, new Vector3(i, j), Quaternion.identity) as GameObject;
                emptyGrid.GetComponent<GridEvent>().setGridCoor(new Vector2(i, j)); // identify the grid as it's coordinate
                tempGridList.Add(emptyGrid); //add one grid to one column
                emptyGrid.transform.SetParent(emptyGridParent.transform); //group instantiated object to a parent
            }
            gridSquareList.Add(tempGridList); //add one column of grid to a row
        }

        //Set turn
        playerXTurn = true;
	}
	
	// Update is called once per frame
	void Update () {

        // Left mouse button event
	    if (Input.GetMouseButton(0)) {
            if (playerXTurn) { InsertPlayerOX(playerXPrefab, "x"); } // Instantiate X
            else { InsertPlayerOX(playerOPrefab, "o"); }
        }
	}

    void InsertPlayerOX (GameObject playerOXPrefab, string playerValue) {

        // Get grid's position and instantiate player OX object
        RaycastHit2D objectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (objectHit.transform != null) {
            if (objectHit.transform.GetComponent<GridEvent>().getGridValue() == null) {
                objectHit.transform.GetComponent<GridEvent>().setGridValue(playerValue); // Fill empty grid
                GameObject playerOX = Instantiate(playerOXPrefab, objectHit.transform.position, Quaternion.identity) as GameObject;
                playerOX.transform.SetParent(playerOXParent.transform); // Group instantiated object to a parent
                playerXTurn = !playerXTurn; //Switch the turn of the players
            }
        }
        checkOtherOX();
    }

    //Temporary solution, bad code
    void checkOtherOX () {
        int rowValue;
        int columnValue;

        // Check each column and row
        for (int x = 0; x < gridSquareList.Count; x++) {
            rowValue = 0;
            columnValue = 0;

            for (int y = 0; y < gridSquareList[x].Count; y++) {
                string columnGridValue = gridSquareList[x][y].GetComponent<GridEvent>().getGridValue();
                string rowGridValue = gridSquareList[y][x].GetComponent<GridEvent>().getGridValue();

                //Increase row/column value if O found, reduce it if X found
                if (columnGridValue == "o") { columnValue++; }
                else if (columnGridValue == "x") { columnValue--; }
                if (rowGridValue == "o") { rowValue++; }
                else if (rowGridValue == "x") { rowValue--; }
            }

            //Win when either O or X has filled a row/column
            if (rowValue == 3 || columnValue == 3) { Debug.Log("Player O Win!"); break; }
            else if (rowValue == -3 || columnValue == -3) { Debug.Log("Player X Win!"); break; }
        }
    }
}
