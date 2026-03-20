using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime = 600f; // 10 นาที

    public TextMeshProUGUI timerText;


    bool isGameOver = false;

    void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    void Update()
    {
        if (isGameOver) return;

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            gameTime = 0;
            EndGame();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void EndGame()
    {
        isGameOver = true;

        int score = ScoreManager.Instance.score;

        // ✅ ดึงชื่อผู้เล่นจาก Login
        string username = PlayerPrefs.GetString("currentUser");

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("NO USER LOGGED IN!");
            return;
        }

        // 💾 เก็บคะแนนล่าสุด
        PlayerPrefs.SetInt("FinalScore", score);

        // 🏆 โหลด HighScore ของ "คนนี้"
        int best = PlayerPrefs.GetInt("HighScore_" + username, 0);

        // 🔥 ถ้าคะแนนใหม่มากกว่า → อัปเดต
        if (score > best)
        {
            PlayerPrefs.SetInt("HighScore_" + username, score);
        } 

        PlayerPrefs.Save();

        // ไป Scene จบเกม
        SceneManager.LoadScene("GameOver");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}