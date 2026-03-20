using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}