using UnityEngine;
using System.Collections;

public class GridEvent : MonoBehaviour {

    string gridFill;
    Vector2 gridCoor;
    
	public void setGridValue(string gridValue) {
        gridFill = gridValue;
    }

    public string getGridValue() {
        return gridFill;
    }

    public void setGridCoor(Vector2 coor) {
        gridCoor = coor;
    }

    public Vector2 getGridCoor() {
        return gridCoor;
    }
}
