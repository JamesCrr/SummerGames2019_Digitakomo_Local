using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    // Public Instance
    public static ScoreUIManager Instance = null;

    [SerializeField]    // Used to access the binary files
    ScoreFileManager fileManager = null;
    [SerializeField]    // Used to enter the new name
    RectTransform inputField = null;

    // The Text Prefab to use
    [SerializeField]
    GameObject textPrefab = null;
    // The Current Score Save Data
    List<ScoreSaveData> currentScores = new List<ScoreSaveData>();
    ScoreSaveData newestData = null;    // To store the newest data
    // Containers to store the scores
    public GameObject nameContainer = null;  
    public GameObject scoreContainer = null;
    // Lists to store the TextObjects
    public List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();    // To Store the Name Texts
    public List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();    // To Store the Score Texts
    int maxCount = 0;       // Used to record how many children text there are
    // String Builder for concatenation
    StringBuilder sb = new StringBuilder();


    // Awake
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        // add the texts into the list to keep track
        foreach (Transform item in nameContainer.transform)
        {
            nameTexts.Add(item.GetComponent<TextMeshProUGUI>());
        }
        foreach (Transform item in scoreContainer.transform)
        {
            scoreTexts.Add(item.GetComponent<TextMeshProUGUI>());
        }
        maxCount = nameTexts.Count;
    }
    // Start
    private void Start()
    {
        fileManager.Single_ClearData();
        //fileManager.Single_Save("Test", 1);
        //fileManager.Single_Save("Test2", 2);
        //fileManager.Single_Save("Test3", 3);
        //fileManager.Single_Save("Test4", 4);
        //fileManager.Single_Save("Test5", 5);

        GetSingleScores();
        SortDescending();
        UpdateUI();

        Single_SaveNewScore(8);
    }




    // Save a new Score to the File
    public void Single_SaveNewScore(int score)
    {
        newestData = new ScoreSaveData("", score);
        currentScores.Add(newestData);
        // Sort it
        SortDescending();
        // Update UI
        UpdateUI(newestData);
    }
    // Load Scores to UI from currentScores List
    void UpdateUI(ScoreSaveData newestData = null)
    {
        // Deactivate all existing texts first
        DeactivateAllText();

        // Add everything back in
        TextMeshProUGUI textCom = null;
        // Loop through the scores
        for(int i = 0; i < currentScores.Count; ++i)
        {
            if (i >= maxCount)
                break;

            // If found the newest score
            if(currentScores[i] == newestData)
            {
                inputField.gameObject.SetActive(true);
                inputField.parent = nameContainer.transform;
            }
            else
            {   // Means that the newest score is just too low
                sb.Clear();
                // Get the text component NAME
                textCom = GetNameText();
                textCom.transform.parent = nameContainer.transform;
                // Assign data
                textCom.text = sb.Append("-  " + currentScores[i].plyrName).ToString();
            }
           

            sb.Clear();
            // Get the text component SCORE
            textCom = GetScoreText();
            textCom.transform.parent = scoreContainer.transform;
            // Assign data
            textCom.text = sb.Append("-  " + currentScores[i].plyrScore).ToString();
        }

        
    }
    // Deactivate all Text Objects
    void DeactivateAllText()
    {
        // Name
        foreach (TextMeshProUGUI item in nameTexts)
        {
            if (item.gameObject.activeSelf == false)
                continue;
            item.transform.parent = null;
            item.gameObject.SetActive(false);
        }
        // Score
        foreach (TextMeshProUGUI item in scoreTexts)
        {
            if (item.gameObject.activeSelf == false)
                continue;
            item.transform.parent = null;
            item.gameObject.SetActive(false);
        }
        // Input Field
        inputField.gameObject.SetActive(false);
    }

    #region Sorting
    // Sort the List from Highest to Lowest
    void SortDescending()
    {
        if(currentScores.Count > 1)
            currentScores.Sort(CompareScore);
    }
    // -1 if second is smaller
    // 0 if equal
    // 1 if second is larger
    int CompareScore(ScoreSaveData first, ScoreSaveData second)
    {
        return -(first.plyrScore.CompareTo(second.plyrScore));
    }
    #endregion

    #region Name Checking
    // Returns true if name Exists
    // Returns false if name doesn't exist
    public bool NameExists(string name)
    {
        foreach (ScoreSaveData item in currentScores)
        {
            if (item.plyrName == name)
                return true;
        }
        return false;   
    }
    #endregion

    #region Object Pooling
    TextMeshProUGUI GetNameText()
    {
        foreach (TextMeshProUGUI item in nameTexts)
        {
            if(item.gameObject.activeSelf == false)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }
    TextMeshProUGUI GetScoreText()
    {
        foreach (TextMeshProUGUI item in scoreTexts)
        {
            if (item.gameObject.activeSelf == false)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Input Field
    public void NameConfirm()
    {
        string newName = inputField.GetComponent<TMP_InputField>().text;

        if (newName == "")      // Empty String?
            return;
        if (NameExists(newName))    // Name already exists?
            return;
        // Save into the binary file
        newestData.plyrName = newName;
        fileManager.Single_Save(newestData.plyrName, newestData.plyrScore);

        // Update UI
        GetSingleScores();
        UpdateUI();
    }
    #endregion
    

    // Loads singleScores from the Binary File
    void GetSingleScores()
    {
        // Get Current Scores
        if(fileManager.Single_Load() != null)
            currentScores = fileManager.Single_Load();
    }
}
