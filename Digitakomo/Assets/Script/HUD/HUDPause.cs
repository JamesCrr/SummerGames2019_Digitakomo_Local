using UnityEngine;

public class HUDPause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
}
