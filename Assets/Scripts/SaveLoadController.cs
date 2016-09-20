using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadController {

    public static int savedXScore, savedOScore, loadedXScore, loadedOScore;

    public void Save() {
        BinaryFormatter formatter = new BinaryFormatter();

        //persistentDataPath is a read-only folder of unity
        //Create file if not exist, overwrite if exist
        FileStream file = File.Create(Application.persistentDataPath + "/data.sav");

        //Store new score into the file
        PlayerScore newScore = new PlayerScore();
        newScore.xScore = savedXScore;
        newScore.oScore = savedOScore;

        formatter.Serialize(file, newScore);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/data.sav")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/data.sav", FileMode.Open);
            PlayerScore loadScore = (PlayerScore)formatter.Deserialize(file);
            file.Close();
            loadedXScore = loadScore.xScore;
            loadedOScore = loadScore.oScore;
        }
    }

    [Serializable]
    class PlayerScore {
        public int xScore;
        public int oScore;
    }
}
