using UnityEngine;
using UnityEngine.UI;

public class HUDTimer : MonoBehaviour
{
    public Timer timer;
    private Text timeText;

    private void Start()
    {
        timeText = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if (timer.GetTimeLeft() > 0)
        {
            timeText.text = Mathf.Ceil(timer.GetTimeLeft()).ToString();
        }
    }
}
