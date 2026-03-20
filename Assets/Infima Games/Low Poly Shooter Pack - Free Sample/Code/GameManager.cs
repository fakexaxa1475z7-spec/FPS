using UnityEngine;
using TMPro; // 👈 สำคัญ
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime = 600f; // 10 นาที

    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    bool isGameOver = false;

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

        // 💾 เก็บคะแนนล่าสุด
        PlayerPrefs.SetInt("FinalScore", score);

        // 🏆 โหลด High Score เดิม
        int best = PlayerPrefs.GetInt("HighScore", 0);

        // 🔥 ถ้าคะแนนใหม่มากกว่า → อัปเดต
        if (score > best)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        // ไป Scene จบเกม
        SceneManager.LoadScene("GameOver");
    }
}