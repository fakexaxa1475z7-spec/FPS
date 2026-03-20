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
        DisplayLeaderboard();
    }

    public void AddScore(string playerName, int score)
    {
        scores.Add(new PlayerScore(playerName, score));

        // เรียงคะแนนมาก → น้อย
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        // เก็บแค่ Top 10
        if (scores.Count > 10)
            scores.RemoveAt(scores.Count - 1);

        SaveScores();
        DisplayLeaderboard();
    }

    void DisplayLeaderboard()
    {
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
            scores = JsonUtility.FromJson<ScoreListWrapper>(json).scores;
        }
    }
}

// wrapper สำหรับ Json
[System.Serializable]
public class ScoreListWrapper
{
    public List<PlayerScore> scores;

    public ScoreListWrapper(List<PlayerScore> scores)
    {
        this.scores = scores;
    }
}