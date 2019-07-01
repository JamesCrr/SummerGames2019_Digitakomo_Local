using UnityEngine;

public class Timer : MonoBehaviour
{
    public float TimeToWin = 300f;

    private float winTime;
    // Start is called before the first frame update
    void Start()
    {
        winTime = Time.time + TimeToWin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= winTime)
        {
            //SceneController.LoadEndScene(true);
        }
    }

    public float GetTimeLeft()
    {
        return winTime - Time.time;
    }
}
