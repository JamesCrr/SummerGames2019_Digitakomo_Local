using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;   // To allow for access to binary files
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFileManager : MonoBehaviour
{
    public int plyrCount = 2;
    
    // To store filePath for save file
    static string saveFilePath;
    static List<ScoreSaveData> listOfScores = new List<ScoreSaveData>();

    private void Awake()
    {
        // Check if we need to use the Single or Multiplayer Path
        if(plyrCount == 2)//if(GameManager.Instance.PlayerCount == 2)
            saveFilePath = Path.Combine(Application.persistentDataPath, "multiScoreData.sc");
        else
            saveFilePath = Path.Combine(Application.persistentDataPath, "singleScoreData.sc");
    }


    // Saving Score into file path
    public void Save(string newName, int newScore)
    {
        // If File already exists, then load existing data first
        if (File.Exists(saveFilePath))
            Load();

        // BinaryFormatter and FileStream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);
        // Convert raw data and add into list
        ScoreSaveData newData = new ScoreSaveData(newName, newScore);
        listOfScores.Add(newData);
        // Serialise into Binary File
        bf.Serialize(stream, listOfScores);

        // Close the stream
        stream.Close();
        return;
    }

    // Loading Single Player Scores
    public List<ScoreSaveData> Load()
    {
        // If File does not exist, return false
        if (!File.Exists(saveFilePath))
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Open);
        // Deserialize the Data and assign to list
        listOfScores = bf.Deserialize(stream) as List<ScoreSaveData>;
        // Close the stream
        stream.Close();

        return listOfScores;
    }
    
    // Clear File
    public void ClearData()
    {
        if (File.Exists(saveFilePath))
            File.Delete(saveFilePath);
    }


}




[System.Serializable]   // To allow unity to read and write from this class
public class ScoreSaveData
{
    public string plyrName;
    public int plyrScore;

    public ScoreSaveData(string name, int score)
    {
        plyrName = name;
        plyrScore = score;
    }
}
