using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;   // To allow for access to binary files
using System.IO;
using UnityEngine;

public class ScoreFileManager : MonoBehaviour
{
    [SerializeField]    // The maximum number of scores to display
    int maxScoreToDisplay = 12;
    // To store filePath for single Player
    static string singlePlayerFilePath;
    static List<ScoreSaveData> listOfScores = new List<ScoreSaveData>();

    private void Awake()
    {
        singlePlayerFilePath = Path.Combine(Application.persistentDataPath, "singleScoreData.sc");
    }

    
    // Saving Single Player Score
    public void Single_SaveNewScore(string newName, int newScore)
    {
        
        // If File already exists, then load existing data first
        if (!File.Exists(singlePlayerFilePath))
            Single_LoadScores();
        // If reached limit, then don't add more
        if (listOfScores.Count >= maxScoreToDisplay)
            return;

        // BinaryFormatter and FileStream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(singlePlayerFilePath, FileMode.Create);
        // Convert raw data and add into list
        ScoreSaveData newData = new ScoreSaveData(newName, newScore);
        listOfScores.Add(newData);
        SortDescending();   // Sort the list
        // Serialise into Binary File
        bf.Serialize(stream, listOfScores);

        // Close the stream
        stream.Close();
        return;
    }

    // Loading Single Player Scores
    public List<ScoreSaveData> Single_LoadScores()
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


    // Sort the List from Highest to Lowest
    void SortDescending()
    {
        listOfScores.Sort(CompareScore);
    }
    // -1 if second is smaller
    // 0 if equal
    // 1 if second is larger
    int CompareScore(ScoreSaveData first, ScoreSaveData second)
    {
        return -(first.plyrScore.CompareTo(second.plyrScore));
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
