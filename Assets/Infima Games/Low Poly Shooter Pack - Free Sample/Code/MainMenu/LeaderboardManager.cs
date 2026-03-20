using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;

    private List<PlayerScore> scores = new List<PlayerScore>();


    void Start()
    {
        LoadScores();

        // 🔥 เช็คว่ามี score ใหม่ไหม
        if (PlayerPrefs.HasKey("FinalScore"))
        {
            string username = PlayerPrefs.GetString("currentUser");

            if (!string.IsNullOrEmpty(username))
            {
                // 👉 ใช้ HighScore (สำคัญมาก)
                int best = PlayerPrefs.GetInt("HighScore_" + username, 0);

                AddScore(username, best);

                Debug.Log("ADD TO LB: " + username + " " + best);
            }

            // 🔥 กัน add ซ้ำ
            PlayerPrefs.DeleteKey("FinalScore");
        }

        DisplayLeaderboard();
    }

    public void AddScore(string playerName, int score)
    {
        bool found = false;

        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].playerName == playerName)
            {
                found = true;

                // 🔥 อัปเดตเฉพาะถ้าคะแนนดีกว่า
                if (score > scores[i].score)
                {
                    scores[i].score = score;
                }

                break;
            }
        }

        // 👉 ถ้ายังไม่มีชื่อ → เพิ่มใหม่
        if (!found)
        {
            scores.Add(new PlayerScore(playerName, score));
        }

        // 🥇 เรียงคะแนนมาก → น้อย
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        // 🎯 Top 10
        if (scores.Count > 10)
            scores.RemoveAt(scores.Count - 1);

        SaveScores();
    }

    // 🔥 ต้องเป็น public
    public void DisplayLeaderboard()
    {
        if (leaderboardText == null)
        {
            Debug.Log("leaderboardText NOT SET!");
            return;
        }

        if (scores == null || scores.Count == 0)
        {
            leaderboardText.text = "No scores yet";
            return;
        }

        leaderboardText.text = "";

        for (int i = 0; i < scores.Count; i++)
        {
            leaderboardText.text += $"{i + 1}. {scores[i].playerName} - {scores[i].score}\n";
        }
    }

    void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper(scores));
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    void LoadScores()
    {
        if (PlayerPrefs.HasKey("Leaderboard"))
        {
            string json = PlayerPrefs.GetString("Leaderboard");

            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);

            if (wrapper != null && wrapper.scores != null)
            {
                scores = wrapper.scores;
            }
            else
            {
                scores = new List<PlayerScore>();
            }
        }
        else
        {
            scores = new List<PlayerScore>();
        }
    }
}

[System.Serializable]
public class ScoreListWrapper
{
    public List<PlayerScore> scores;

    public ScoreListWrapper(List<PlayerScore> scores)
    {
        this.scores = scores;
    }
}