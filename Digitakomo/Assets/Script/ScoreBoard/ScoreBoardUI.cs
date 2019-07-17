using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreBoardUI : MonoBehaviour
{
    // Public Instance
    public static ScoreBoardUI Instance = null;

    [Header("Backgrounds")]     // To store the different backgrounds for Single or Muliplayer
    [SerializeField]
    Sprite single = null;
    [SerializeField]
    Sprite multi = null;
    [SerializeField]
    Image background = null;       // The background to display the sprites

    [Header("File Manager")]
    [SerializeField]    // Used to access the binary files
    ScoreFileManager fileManager = null;
    [Header("Input Field")]
    [SerializeField]    // Used to enter the new name
    RectTransform inputField = null;

    [Header("UI Stuff")]
    // The Text Prefab to use
    [SerializeField]
    GameObject textPrefab = null;
    // The Current Score Save Data
    List<ScoreSaveData> currentScores = new List<ScoreSaveData>();
    ScoreSaveData newestData = null;    // To store the newest data
    // Containers to store the scores
    [SerializeField]
    GameObject nameContainer = null;
    [SerializeField]
    GameObject scoreContainer = null;
    // Lists to store the TextObjects
    List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();    // To Store the Name Texts
    List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();    // To Store the Score Texts
    int maxCount = 0;       // Used to record how many children text there are
    // String Builder for concatenation
    StringBuilder sb = new StringBuilder();
    [Header("Buttons")]
    [SerializeField]         // Button to clear current HighScores
    GameObject clearButton = null;
    [SerializeField]         // Button to move on to next Scene
    GameObject continueButton = null;


    // Awake
    private void Awake()
    {
        if (Instance != null)
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

        // Check if we need to use the Single or Multiplayer Background Image
        if (fileManager.plyrCount == 2)//if (GameManager.Instance.PlayerCount == 2)
            background.sprite = multi;
        else
            background.sprite = single;

        // Disable Buttons
        clearButton.SetActive(false);
        continueButton.SetActive(false);
    }



    #region File Manager Functions
    // Input a new Score to the File and waits for a name to be inputed
    public void InputNewScore(int score)
    {
        newestData = new ScoreSaveData("", score);
        currentScores.Add(newestData);
        // Sort it
        SortDescending();
        // Update UI
        if (!UpdateUI(newestData))
        {
            // Buttons
            clearButton.SetActive(true);
            continueButton.SetActive(true);
        }
    }
    // Load Scores from the Binary File
    void GetScores()
    {
        // Get Current Scores
        if (fileManager.Load() != null)
            currentScores = fileManager.Load();
        else
            currentScores.Clear();
    }
    // Remove all Scores from the Binary file and UI
    public void RemoveScores()
    {
        fileManager.ClearData();
        GetScores();
        UpdateUI();
    }
    #endregion

    #region UI
    // Load Scores to UI from currentScores List
    // Pass in the newest score to attach a Input field to that name
    // Returns whether the newest Data is on the HighScore or if it's too low to even appear
    bool UpdateUI(ScoreSaveData newestData = null)
    {
        // Deactivate all existing texts first
        DeactivateAllText();

        // Add everything back in
        TextMeshProUGUI textCom = null;
        bool foundScore = false;
        // Loop through the scores
        for (int i = 0; i < currentScores.Count; ++i)
        {
            if (i >= maxCount)
                break;

            // If found the newest score
            if (currentScores[i] == newestData)
            {
                inputField.gameObject.SetActive(true);  // Enable the Input Field
                inputField.SetParent(nameContainer.transform);
                foundScore = true;
            }
            else
            {   // Means that the newest score is just too low
                sb.Clear();
                // Get the text component NAME
                textCom = GetNameText();
                textCom.transform.SetParent(nameContainer.transform);
                // Assign data
                textCom.text = sb.Append("-  " + currentScores[i].plyrName).ToString();
            }


            sb.Clear();
            // Get the text component SCORE
            textCom = GetScoreText();
            textCom.transform.SetParent(scoreContainer.transform);
            // Assign data
            textCom.text = sb.Append("-  " + currentScores[i].plyrScore).ToString();
        }

        return foundScore;
    }

    public void Continue()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Deactivate all Text Objects
    void DeactivateAllText()
    {
        // Name
        foreach (TextMeshProUGUI item in nameTexts)
        {
            if (item.gameObject.activeSelf == false)
                continue;
            item.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }
        // Score
        foreach (TextMeshProUGUI item in scoreTexts)
        {
            if (item.gameObject.activeSelf == false)
                continue;
            item.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }
        // Input Field
        inputField.gameObject.SetActive(false);
    }

    // Sets old Data to UI
    public void RetrieveExistingData()
    {
        // Get Existing Scores
        GetScores();
        SortDescending();
        UpdateUI();
    }
    #endregion

    #region Sorting
    // Sort the List from Highest to Lowest
    void SortDescending()
    {
        if (currentScores.Count > 1)
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
            if (item.gameObject.activeSelf == false)
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
        fileManager.Save(newestData.plyrName, newestData.plyrScore);

        // Update UI
        GetScores();
        UpdateUI();

        // Buttons
        clearButton.SetActive(true);
        continueButton.SetActive(true);
    }
    #endregion



    // Returns false if there are 2 players playing
    // Returns true if there is only 1 player playing
    bool IsSinglePlayer()
    {
        return FindObjectOfType<CrossScene>().PlayerCount == 1;
    }
}
