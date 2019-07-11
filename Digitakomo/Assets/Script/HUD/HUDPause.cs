using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDPause : MonoBehaviour
{
    private bool isPaused = false;
    private float defaultTimeScale;
    public GameObject pauseObject;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = defaultTimeScale;
        pauseObject.SetActive(false);
    }

    private void Pause()
    {
        isPaused = true;
        defaultTimeScale = Time.timeScale;
        Time.timeScale = 0;
        pauseObject.SetActive(true);
    }

    public void Restart()
    {
        Resume();
        SceneController._LoadSceneWithLoadingScreen(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Resume();
        SceneController._LoadSceneWithLoadingScreen("MainMenu");
    }
}
