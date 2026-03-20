using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI playerNameText;

    bool addedToLeaderboard = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int score = PlayerPrefs.GetInt("FinalScore", 0);
        string username = PlayerPrefs.GetString("currentUser");

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("NO USER LOGGED IN!");
            return;
        }

        // ✅ HighScore แยก user
        int highScore = PlayerPrefs.GetInt("HighScore_" + username, 0);

        // 🔥 เพิ่มเข้า Leaderboard (สำคัญมาก)
        if (!addedToLeaderboard)
        {
            LeaderboardManager lb = FindObjectOfType<LeaderboardManager>();
            if (lb != null)
            {
                lb.AddScore(username, score);
                Debug.Log("Added to leaderboard: " + username + " " + score);
            }
            else
            {
                Debug.Log("LeaderboardManager NOT FOUND in GameOver Scene");
            }

            addedToLeaderboard = true;
        }

        playerNameText.text = "Player: " + username;
        scoreText.text = "Score: " + score;
        highScoreText.text = "Best: " + highScore;
    }
}