using UnityEngine;
using System.Collections;

public class GridEvent : MonoBehaviour {

    int gridValue;
    
    void Start() {
        gridValue = 0;
    }

    public void setGridValue(int value) {
        gridValue = value;
    }

    public int getGridValue() {
        return gridValue;
    }
}
