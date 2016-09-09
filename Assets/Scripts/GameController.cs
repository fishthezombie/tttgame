using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject gameEnvironment;
    public Text topUIMessage;
    public string firstMessage;
    public RawImage victoryBGImage;
    public Text victoryTextMessage;

    public GameObject playerXPrefab;
    public string playerXMark;
    public GameObject playerOPrefab;
    public string playerOMark;
    public bool XGoesFirst;

    public GameObject emptyGridPrefab;
    public float gridSize;
    public float spaceBetweenGrids;
    public List<List<GameObject>> gridSquareList;

    GameObject emptyGridParent, playerOXParent;
    bool gameEnd;
    int emptyGridCount;

    //Initialization
    void Start () {

        //Initialize Parent for instantiated prefabs
        emptyGridParent = new GameObject();
        emptyGridParent.name = "Empty Grid Parent";
        playerOXParent = new GameObject();
        playerOXParent.name = "Player OX Parent";

        //Generate game interface
        GameObject environment = Instantiate(gameEnvironment, this.transform.position, Quaternion.identity) as GameObject;
        environment.transform.SetParent(this.transform);
        topUIMessage.text = firstMessage;
        victoryBGImage.enabled = false;

        //Generate grids
        gridSquareList = new List<List<GameObject>>();
        List<GameObject> tempGridList;
	    for (float i = 0; i < (gridSize * spaceBetweenGrids); i = i + spaceBetweenGrids) {
            tempGridList = new List<GameObject>();
            for (float j = 0; j < (gridSize * spaceBetweenGrids); j = j + spaceBetweenGrids) {
                GameObject emptyGrid = Instantiate(emptyGridPrefab, new Vector3(i, j), Quaternion.identity) as GameObject;
                emptyGrid.GetComponent<GridEvent>().setGridCoor(new Vector2(i, j)); // identify the grid as it's coordinate
                tempGridList.Add(emptyGrid); //add one grid to one column
                emptyGrid.transform.SetParent(emptyGridParent.transform); //group instantiated object to a parent
            }
            gridSquareList.Add(tempGridList); //add one column of grid to a row
        }
        emptyGridCount = (int)(gridSize * gridSize);
	}
	
	// Update is called once per frame
	void Update () {

        // Left mouse button event
	    if (Input.GetMouseButtonDown(0)) {
            if (topUIMessage.text != "") topUIMessage.text = ""; //Remove game first message
            if (XGoesFirst) { InsertPlayerOX(playerXPrefab, playerXMark); } // Instantiate X
            else { InsertPlayerOX(playerOPrefab, playerOMark); }
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
                XGoesFirst = !XGoesFirst; //Switch the turn of the players
            }
        }

        if (!gameEnd) checkOtherOX();
    }

    //Temporary solution, bad code
    void checkOtherOX () {
        int rowValue, columnValue, diagonalUpValue, diagonalDownValue;

        // Check each column and row
        for (int x = 0; x < gridSquareList.Count; x++) {
            rowValue = 0;
            columnValue = 0;
            diagonalUpValue = 0;
            diagonalDownValue = 0;

            for (int y = 0; y < gridSquareList[x].Count; y++) {
                string columnGridValue = gridSquareList[x][y].GetComponent<GridEvent>().getGridValue();
                string rowGridValue = gridSquareList[y][x].GetComponent<GridEvent>().getGridValue();
                string diagonalUpGridValue = gridSquareList[y][y].GetComponent<GridEvent>().getGridValue();
                string diagonalDownGridValue = gridSquareList[y][gridSquareList[x].Count - 1 - y].GetComponent<GridEvent>().getGridValue();

                //Increase row/column value if O found, reduce it if X found
                if (columnGridValue == playerOMark) { columnValue++; }
                else if (columnGridValue == playerXMark) { columnValue--; }
                if (rowGridValue == playerOMark) { rowValue++; }
                else if (rowGridValue == playerXMark) { rowValue--; }
                if (diagonalUpGridValue == playerOMark) { diagonalUpValue++; }
                else if (diagonalUpGridValue == playerXMark) { diagonalUpValue--; }
                if (diagonalDownGridValue == playerOMark) { diagonalDownValue++; }
                else if (diagonalDownGridValue == playerXMark) { diagonalDownValue--; }
            }

            //Win when either O or X has filled a row/column
            if (rowValue == (int)gridSize || 
                columnValue == (int)gridSize || 
                diagonalUpValue == (int)gridSize || 
                diagonalDownValue == (int)gridSize) {
                    VictoryConditionMet(playerOMark);
            }
            else if (rowValue == (int)-gridSize || 
                columnValue == (int)-gridSize || 
                diagonalUpValue == (int)-gridSize || 
                diagonalDownValue == (int)-gridSize) {
                    VictoryConditionMet(playerXMark);
            }

            //Otherwise tie
            else if (emptyGridCount == 0) VictoryConditionMet("No One");

        }
        emptyGridCount--; //decrease empty grid count
    }

    void VictoryConditionMet(string victoriousPlayer)
    {
        GameObject[] gridToDestroy = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in gridToDestroy) Destroy(grid);

        //enable victory UI
        victoryBGImage.enabled = true;
        victoryTextMessage.text = victoriousPlayer + " Won!";

        //mark the game has ended
        gameEnd = true;
    }
}
