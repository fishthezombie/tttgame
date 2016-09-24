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
    SaveLoadController saveLoad;
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
	    for (float i = 0; i < gridSize; ++i) {
            tempGridList = new List<GameObject>();
            for (float j = 0; j < gridSize; ++j) {
                GameObject emptyGrid = Instantiate(emptyGridPrefab, new Vector3(i * spaceBetweenGrids, j * spaceBetweenGrids), Quaternion.identity) as GameObject;
                emptyGrid.GetComponent<GridEvent>().setGridPos((int)(i), (int)(j)); //Set the grid index position
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
        saveLoad = new SaveLoadController();
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

            //Get the scripts of the raycasted object
            GridEvent gridEvent = objectHit.transform.GetComponent<GridEvent>();

            //Insert the sprite based on turn
            if (gridEvent.getGridValue() == 0) {  //If gridValue still 0, means the grid is still empty
                gameVisual.InsertMark(objectHit.transform.gameObject, xTurn); //Insert the sprite of the current player to the grid
                if (xTurn)
                    gridEvent.setGridValue(xValue);
                else
                    gridEvent.setGridValue(oValue);
                emptyGridCount--; //Decrease empty grid count
                xTurn = !xTurn; //Switch turn to the other player
				if (!gameEnd) checkWinner(gridEvent.getGridPos());
            }
        }
    }

    void checkWinner (Vector2 gridPos) {
        int rowValue, columnValue, diagonalUpValue, diagonalDownValue;
        int xTotal = xValue * (int)gridSize,
            oTotal = oValue * (int)gridSize;
        int gridX = (int)gridPos.x,
            gridY = (int)gridPos.y;

        //initialize value
        rowValue = 0;
        columnValue = 0;
        diagonalUpValue = 0;
        diagonalDownValue = 0;

        // Check the column, row, and diagonals of the marked grid
        // Only check diagonals if it's in the diagonal line
        for (int x = 0; x < gridSquareList.Count; ++x) { // Check column and row
            columnValue = columnValue +
                gridSquareList[gridX][x].GetComponent<GridEvent>().getGridValue();
            rowValue = rowValue +
                gridSquareList[x][gridY].GetComponent<GridEvent>().getGridValue();
        }
                //If winner found when checking row or column, no need to check diagonals
        if (columnValue == oTotal || rowValue == oTotal) {
            GameWinner(oTotal);
        } else if(columnValue == xTotal || rowValue == xTotal) {
            GameWinner(xTotal);
        } else {
            //Check diagonals only if possible
            if (gridX == gridY) {
                for (int x = 0; x < gridSquareList.Count; ++x) {
                    diagonalUpValue = diagonalUpValue +
                        gridSquareList[x][x].GetComponent<GridEvent>().getGridValue();
                }
            }
            if (gridX == (gridSquareList.Count - 1 - gridY)) {
                for (int x = 0; x < gridSquareList.Count; ++x) {
                    diagonalDownValue = diagonalDownValue +
                        gridSquareList[x][gridSquareList.Count - 1 - x].GetComponent<GridEvent>().getGridValue();
                }
            }
        }

        //Declare winner if found on diagonals
        if (diagonalUpValue == oTotal || diagonalDownValue == oTotal)
            GameWinner(oTotal);
        else if (diagonalUpValue == xTotal || diagonalDownValue == xTotal)
            GameWinner(xTotal);

        //Check if all grid has been filled and the game hasn't ended
        if (emptyGridCount == 0 && !gameEnd)
            GameWinner(0);
    }

    void GameWinner(int oxValue) {
        //mark the game has ended
        gameEnd = true;

        saveLoad.Load(); // Load the score first to get the previous score

        //enable victory UI and increase the score of winning player
        if (oxValue == (xValue * (int)gridSize)) {
            gameVisual.WinGameVisual("X Won!");
            SaveLoadController.savedXScore = ++SaveLoadController.loadedXScore; 
        } else if (oxValue == (oValue * (int)gridSize)) {
            gameVisual.WinGameVisual("O Won!");
            SaveLoadController.savedOScore = ++SaveLoadController.loadedOScore;
        } else
            gameVisual.WinGameVisual("It's a Draw!");

        //Save the game with the new score
        saveLoad.Save();

        //Update score board with the new score
        gameVisual.UpdateScoreBoard();
    }
}
