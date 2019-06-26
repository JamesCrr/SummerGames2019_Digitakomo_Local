using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static bool IsWin = false;

    [SerializeField]
    string StartGameSceneName = "GameScene";

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene(StartGameSceneName);
    }

    public static void LoadEndScene(bool IsWin)
    {
        SceneController.IsWin = IsWin;
        SceneManager.LoadScene("EndScene");
    }
}
