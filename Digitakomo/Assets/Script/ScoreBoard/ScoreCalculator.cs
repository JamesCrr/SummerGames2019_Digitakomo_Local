using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCalculator : MonoBehaviour
{
    // Public Instance
    public static ScoreCalculator Instance = null;

    // What type of Score to add
    public enum SCORE_TYPE
    {
        PT1_DIE,
        PT1_POISON,

        PT2_DIE,
        PT2_FIRE,

        APEWOLF_DIE,
        APEWOLF_ROCKS,

        SQWOLF_DIE,
        SQWOLF_ACID,

        ST_NONE,
    }
    int score = 0;



    // Awake
    private void Awake()
    {
        // Attach Instance
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        // Add Scene Loading Event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name != "SaveTest")
            return;
        // Input new Score
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreBoardUI>().InputNewScore(score);
        // Clear the Score
        ClearScore();
    }



    // Add Score
    public void AddScore(SCORE_TYPE type)
    {
        int amount = 0;
        switch (type)
        {
            case SCORE_TYPE.PT1_DIE:        // 10 Points
            case SCORE_TYPE.PT2_DIE:
            case SCORE_TYPE.APEWOLF_ROCKS:
            case SCORE_TYPE.SQWOLF_ACID:
                amount = 10;
                break;
            case SCORE_TYPE.PT1_POISON:        // 1 Points
            case SCORE_TYPE.PT2_FIRE:
                amount = 1;
                break;
            case SCORE_TYPE.APEWOLF_DIE:       // 50 Points
                amount = 50;
                break;
            case SCORE_TYPE.SQWOLF_DIE:        // 30 Points
                amount = 30;
                break;
        }
        score += amount;
    }
    // Get Score
    public int GetScore()
    {
        return score;
    }
    // Clear Score
    public void ClearScore()
    {
        score = 0;
    }
}
