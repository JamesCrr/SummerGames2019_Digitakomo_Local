using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    // Public Instance
    public static ScoreUIManager Instance = null;

    [SerializeField]
    ScoreFileManager fileManager = null;

    // The Score and Name Panel 
    [SerializeField]
    GameObject scorePanel = null;
    [SerializeField]
    GameObject namePanel = null;
    // The Text Prefab to use
    [SerializeField]
    GameObject text;
    // Lists to store the TextObjects 
    List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();    // To Store the Name Texts
    List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();    // To Store the Score Texts

    // Awake
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        // Get the Name and Score texts
        for(int i = 0; i < namePanel.transform.childCount; ++i)
        {
            nameTexts.Add(namePanel.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
            scoreTexts.Add(scorePanel.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }
    // Start
    private void Start()
    {
        fileManager.Single_SaveNewScore("wow,", 132);
        fileManager.Single_SaveNewScore("wagw", 1322);
        fileManager.Single_SaveNewScore("wqwe", 1302);
        fileManager.Single_SaveNewScore("aszxczxc", 52);

        fileManager.Single_SaveNewScore("wow,", 132);
        fileManager.Single_SaveNewScore("wagw", 1322);
        fileManager.Single_SaveNewScore("wqwe", 1302);
        fileManager.Single_SaveNewScore("aszxczxc", 52);

        fileManager.Single_SaveNewScore("wow,", 132);
        fileManager.Single_SaveNewScore("wagw", 1322);
        fileManager.Single_SaveNewScore("wqwe", 1302);
        fileManager.Single_SaveNewScore("aszxczxc", 52);

        fileManager.Single_SaveNewScore("wow,", 132);
        fileManager.Single_SaveNewScore("wagw", 1322);
        fileManager.Single_SaveNewScore("wqwe", 1302);
        fileManager.Single_SaveNewScore("aszxczxc", 52);

        LoadSingleScores();
    }


    // Load Single Scores to UI
    public void LoadSingleScores()
    {
        List<ScoreSaveData> data = fileManager.Single_LoadScores();

        // Loop through the textObjects and assign the data
        for(int i = 0; i < nameTexts.Count; ++i)
        {
            nameTexts[i].text += data[i].plyrName;
            scoreTexts[i].text += data[i].plyrScore;
        }
    }
}
