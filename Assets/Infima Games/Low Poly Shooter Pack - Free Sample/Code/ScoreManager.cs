using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;

    void Awake()
    {
        // ✅ กันมีหลายตัว
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ไม่โดนลบตอนเปลี่ยน Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ➕ เพิ่มคะแนน
    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }

    // 🔄 รีเซ็ตคะแนน (สำคัญมากตอนเริ่มเกมใหม่)
    public void ResetScore()
    {
        score = 0;
    }

    // 📥 ดึงคะแนน
    public int GetScore()
    {
        return score;
    }
}