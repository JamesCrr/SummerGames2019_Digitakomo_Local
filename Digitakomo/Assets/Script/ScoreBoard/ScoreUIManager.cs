using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreBoardPanels
    {
        public GameObject nameContainer = null;
        public GameObject scoreContainer = null;
        // Lists to store the TextObjects
        //public List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();    // To Store the Name Texts
        //public List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();    // To Store the Score Texts
    }

    // Public Instance
    public static ScoreUIManager Instance = null;

    [SerializeField]    // Used to access the binary files
    ScoreFileManager fileManager = null;

    // The Score and Name Panel for singlePlayers
    public ScoreBoardPanels scorePanels = new ScoreBoardPanels();
    // The Text Prefab to use
    [SerializeField]
    GameObject textPrefab = null;
    // The Current Score Save Data
    List<ScoreSaveData> currentScores = new List<ScoreSaveData>();
    // All the text Objects
    List<TextMeshProUGUI> listOfTextObj = new List<TextMeshProUGUI>();
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
        

        GameObject newTextObj = null;
        TextMeshProUGUI textCom = null;
        // Create 20 inactive textMeshPro first
        for (int i = 0; i < 20; i++)
        {
            // Instantiate new text and Deactivate it first
            newTextObj = Instantiate(textPrefab);
            textCom = newTextObj.GetComponent<TextMeshProUGUI>();
            listOfTextObj.Add(textCom);
            textCom.gameObject.SetActive(false);
            // Instantiate new text and Deactivate it first
            newTextObj = Instantiate(textPrefab);
            textCom = newTextObj.GetComponent<TextMeshProUGUI>();
            listOfTextObj.Add(textCom);
            textCom.gameObject.SetActive(false);
        }
       
    }
    // Start
    private void Start()
    {
        //fileManager.Single_Save("Test", 1);
        //fileManager.Single_Save("Test2", 2);
        //fileManager.Single_Save("Test3", 3);
        //fileManager.Single_Save("Test4", 4);
        //fileManager.Single_Save("Test5", 5);

        // CHECK HERE IF MULTIPAYER OR SINGLE
        // Get Current Scores
        currentScores = fileManager.Single_Load();
        SortDescending();
        UpdateUI();

        Single_SaveNewScore(10);
    }




    // Save a new Score to the File
    public void Single_SaveNewScore(int score)
    {
        // Create new data and add into list
        TextMeshProUGUI assignedText = null;
        ScoreSaveData newData = new ScoreSaveData("", score);
        currentScores.Add(newData);
        // Sort it
        SortDescending();
        // Update UI and get the text object for the name
        assignedText = UpdateUI(newData);


    }
    // Load Scores to UI from currentScores List
    public TextMeshProUGUI UpdateUI(ScoreSaveData newestData = null)
    {
        // Deactivate all existing texts first
        DeactivateAllText();

        TextMeshProUGUI foundText = null;
        // Add everything back in
        GameObject fetchedGO = null;
        TextMeshProUGUI textCom = null;
        foreach (ScoreSaveData item in currentScores)
        {
            sb.Clear();
            // Name
            fetchedGO = GetText();
            fetchedGO.transform.parent = scorePanels.nameContainer.transform;
            textCom = fetchedGO.GetComponent<TextMeshProUGUI>();
            textCom.text = sb.Append("-  " + item.plyrName).ToString();
            // Check if any item is the same as the newestData
            if (item == newestData)
                foundText = textCom;


            sb.Clear();
            // Score
            fetchedGO = GetText();
            fetchedGO.transform.parent = scorePanels.scoreContainer.transform;
            textCom = fetchedGO.GetComponent<TextMeshProUGUI>();
            textCom.text = sb.Append("-  " + item.plyrScore).ToString();
        }

        return foundText;
    }
    // Deactivate all Text Objects
    void DeactivateAllText()
    {
        foreach (TextMeshProUGUI item in listOfTextObj)
        {
            if (item.gameObject.activeSelf == false)
                continue;
            item.transform.parent = null;
            item.gameObject.SetActive(false);
        }
    }

    #region Sorting
    // Sort the List from Highest to Lowest
    void SortDescending()
    {
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

    #region Text Pooling
    public GameObject GetText()
    {
        foreach (TextMeshProUGUI item in listOfTextObj)
        {
            if (item.gameObject.activeSelf == false)
            {
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }
        // Create new Text
        GameObject newText = Instantiate(textPrefab);
        listOfTextObj.Add(newText.GetComponent<TextMeshProUGUI>());
        return newText;
    }
    #endregion
}
