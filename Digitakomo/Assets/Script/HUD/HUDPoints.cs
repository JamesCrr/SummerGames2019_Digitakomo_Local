using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPoints : MonoBehaviour
{
    Text pointsText;
    int currentScore = 0;

    // Start is called before first frame update
    private void Start()
    {
        pointsText = GetComponent<Text>();
        currentScore = ScoreCalculator.Instance.GetScore();
    }

    private void FixedUpdate()
    {
        // Has the Score Increased?
        if (currentScore != ScoreCalculator.Instance.GetScore())
        {
            currentScore++;
            pointsText.text = currentScore.ToString();
        }
    }
}
