using UnityEngine;
using System.Collections;

public class GridEvent : MonoBehaviour {

    int gridValue, gridXPos, gridYPos;
    
    void Start() {
        gridValue = 0;
    }

    public void setGridValue(int value) {
        gridValue = value;
    }

    public int getGridValue() {
        return gridValue;
    }

    public void setGridPos (int xPos, int yPos) {
        gridXPos = xPos;
        gridYPos = yPos;
    }

    public Vector2 getGridPos() {
        return new Vector2(gridXPos, gridYPos);
    }
}
