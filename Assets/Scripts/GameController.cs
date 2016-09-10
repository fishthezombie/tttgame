using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int xValue;
    public int oValue;
    public bool firstTurnIsX;

    public GameObject emptyGridPrefab;
    public float gridSize;
    public float spaceBetweenGrids;
    public List<List<GameObject>> gridSquareList;

    GameObject emptyGridParent, playerOXParent;
    GameVisualController gameVisual;
    bool gameEnd;
    int emptyGridCount;
    bool xTurn;

    //Initialization
    void Start () {

        //Initialize Parent for instantiated prefabs
        emptyGridParent = new GameObject();
        emptyGridParent.name = "Empty Grid Parent";
        playerOXParent = new GameObject();
        playerOXParent.name = "Player OX Parent";

        //Generate grids
        gridSquareList = new List<List<GameObject>>();
        List<GameObject> tempGridList;
	    for (float i = 0; i < (gridSize * spaceBetweenGrids); i = i + spaceBetweenGrids) {
            tempGridList = new List<GameObject>();
            for (float j = 0; j < (gridSize * spaceBetweenGrids); j = j + spaceBetweenGrids) {
                GameObject emptyGrid = Instantiate(emptyGridPrefab, new Vector3(i, j), Quaternion.identity) as GameObject;
                tempGridList.Add(emptyGrid); //add one grid to one column
                emptyGrid.transform.SetParent(emptyGridParent.transform); //group instantiated object to a parent
            }
            gridSquareList.Add(tempGridList); //add one column of grid to a row
        }
        emptyGridCount = (int)(gridSize * gridSize);

        //Other initialization
        xTurn = firstTurnIsX;
        gameVisual = gameObject.GetComponent<GameVisualController>();
        gameEnd = false;
    }

    // Update is called once per frame
    void Update () {

        // Left mouse button event
	    if (Input.GetMouseButtonDown(0) && !gameEnd) {
            InsertPlayerOX();
        }
	}

    void InsertPlayerOX () {

        // Get grid's position and instantiate player OX object
        RaycastHit2D objectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (objectHit.transform != null) {

            //Get scripts
            GridEvent gridEvent = objectHit.transform.GetComponent<GridEvent>();

            //Insert the sprite based on turn
            if (gridEvent.getGridValue() == 0) {
                gameVisual.InsertMark(objectHit.transform.gameObject, xTurn);
                if (xTurn)
                    gridEvent.setGridValue(xValue);
                else
                    gridEvent.setGridValue(oValue);
                emptyGridCount--; //decrease empty grid count
                xTurn = !xTurn; //Switch turn
            }
        }

        if (!gameEnd) checkWinner();
    }

    //Temporary solution, bad code
    void checkWinner () {
        int rowValue, columnValue, diagonalUpValue, diagonalDownValue;
        int xTotal = xValue * (int)gridSize,
            oTotal = oValue * (int)gridSize;

        // Check each column, row, and diagonals
        for (int x = 0; x < gridSquareList.Count; x++) {
            rowValue = 0;
            columnValue = 0;
            diagonalUpValue = 0;
            diagonalDownValue = 0;
            for (int y = 0; y < gridSquareList[x].Count; y++) {
                columnValue = columnValue
                    + gridSquareList[x][y].GetComponent<GridEvent>().getGridValue();
                rowValue = rowValue
                    + gridSquareList[y][x].GetComponent<GridEvent>().getGridValue();
                diagonalUpValue = diagonalUpValue
                    + gridSquareList[y][y].GetComponent<GridEvent>().getGridValue();
                diagonalDownValue = diagonalDownValue
                    + gridSquareList[y][gridSquareList[x].Count - 1 - y].GetComponent<GridEvent>().getGridValue();
           }

            // If one of the sequence is all filled, decide on the winner
            if (columnValue == xTotal ||
                rowValue == xTotal ||
                diagonalUpValue == xTotal ||
                diagonalDownValue == xTotal)
                GameWinner(xTotal);

            else if (columnValue == oTotal ||
                rowValue == oTotal ||
                diagonalUpValue == oTotal ||
                diagonalDownValue == oTotal)
                GameWinner(oTotal);
        }
        //Otherwise draw when there's no empty grid left
        if (emptyGridCount == 0 && !gameEnd)
            GameWinner(0);

    }


    void GameWinner(int oxValue) {

        //enable victory UI
        if (oxValue == (xValue * (int)gridSize))
            gameVisual.WinGameVisual("X Won!");
        else if (oxValue == (oValue * (int)gridSize))
            gameVisual.WinGameVisual("O Won!");
        else
            gameVisual.WinGameVisual("It's a Draw!");

        //mark the game has ended
        gameEnd = true;
    }
}
