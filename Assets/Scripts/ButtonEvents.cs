using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour {

    SaveLoadController saveLoad;

    void Start() {
        saveLoad = new SaveLoadController();
    }

	//Change the scene to another scene when the button is clicked
    public void ChangeToScene (string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //Reset the current scene when the button is clicked
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetScore() {
        SaveLoadController.savedOScore = 0;
        SaveLoadController.savedXScore = 0;
        saveLoad.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}