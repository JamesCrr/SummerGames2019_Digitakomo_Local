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

    // Awake
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    // Start
    private void Start()
    {
        fileManager.Single_SaveNewScore("wow,", 132);
        fileManager.Single_SaveNewScore("wagw", 1322);
        fileManager.Single_SaveNewScore("wqwe", 1302);
        fileManager.Single_SaveNewScore("aszxczxc", 52);

        LoadSingleScores();
    }


    // Load Single Scores to UI
    public void LoadSingleScores()
    {
        GameObject newTextObj = null;
        //Text textCom = null;
        TextMeshProUGUI textCom = null;
        List<ScoreSaveData> data = fileManager.Single_LoadScores();

        foreach (ScoreSaveData item in data)
        {
            // Instantiate new text and parent under Name Panel
            newTextObj = Instantiate(text);
            newTextObj.transform.parent = namePanel.transform;
            textCom = newTextObj.GetComponent<TextMeshProUGUI>();
            // Add the name
            textCom.text += item.plyrName;

            // Instantiate new text and parent under Score Panel
            newTextObj = Instantiate(text);
            newTextObj.transform.parent = scorePanel.transform;
            textCom = newTextObj.GetComponent<TextMeshProUGUI>();
            // Add the name
            textCom.text += item.plyrScore.ToString();
        }

    }
}
