using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;   // To allow for access to binary files
using System.IO;
using UnityEngine;

public class ScoreFileManager : MonoBehaviour
{
    // To store filePath for single Player
    static string singlePlayerFilePath;
    static List<ScoreSaveData> listOfScores = new List<ScoreSaveData>();

    private void Awake()
    {
        singlePlayerFilePath = Path.Combine(Application.persistentDataPath, "singleScoreData.sc");
    }

    
    // Saving Single Player Score
    public void Single_Save(string newName, int newScore)
    {    
        // If File already exists, then load existing data first
        if (!File.Exists(singlePlayerFilePath))
            Single_Load();

        // BinaryFormatter and FileStream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(singlePlayerFilePath, FileMode.Create);
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
    public List<ScoreSaveData> Single_Load()
    {
        // If File does not exist, return false
        if (!File.Exists(singlePlayerFilePath))
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(singlePlayerFilePath, FileMode.Open);
        // Deserialize the Data and assign to list
        listOfScores = bf.Deserialize(stream) as List<ScoreSaveData>;
        // Close the stream
        stream.Close();

        return listOfScores;
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
