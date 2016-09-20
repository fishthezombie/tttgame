using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreMenuDisplay : MonoBehaviour {

    public Text oDigitFront, oDigitBack, xDigitFront, xDigitBack;
    SaveLoadController saveLoad;
    // Use this for initialization
    void Start() {
        UpdateScoreBoard();
    }

    public void UpdateScoreBoard() {
        //Load score
        saveLoad = new SaveLoadController();
        saveLoad.Load();

        //Shows 2 digits numbers max
        string oScore = Mathf.Clamp(SaveLoadController.loadedOScore, 0, 99).ToString();
        string xScore = Mathf.Clamp(SaveLoadController.loadedXScore, 0, 99).ToString();

        //Properly display the score on the number board
        
        if (oScore.Length > 1) {
            oDigitBack.text = oScore[1].ToString();
            oDigitFront.text = oScore[0].ToString();
        } else {
            oDigitFront.text = "0";
            oDigitBack.text = oScore[0].ToString();
        }

        if (xScore.Length > 1) {
            xDigitBack.text = xScore[1].ToString();
            xDigitFront.text = xScore[0].ToString();
        } else {
            xDigitFront.text = "0";
            xDigitBack.text = xScore[0].ToString();
        }
    }
}
