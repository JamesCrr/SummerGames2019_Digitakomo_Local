using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDPause : MonoBehaviour
{
    private bool isPaused = false;
    private float defaultTimeScale;
    private RawImage ri;

    // Start is called before the first frame update
    void Start()
    {
        ri = GetComponentInChildren<RawImage>(true);
    }

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
            isPaused = !isPaused;
        }
    }

    public void Resume()
    {
        Time.timeScale = defaultTimeScale;
        ri.gameObject.SetActive(false);
    }

    private void Pause()
    {
        defaultTimeScale = Time.timeScale;
        Time.timeScale = 0;
        ri.gameObject.SetActive(true);
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
}
