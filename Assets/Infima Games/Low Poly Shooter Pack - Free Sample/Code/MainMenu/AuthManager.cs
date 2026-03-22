using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Login UI")]
    public TMP_InputField loginUsername;
    public TextMeshProUGUI loginMessage;

    // 🔑 Login (ใส่ชื่อแล้วเข้าเลย)
    public void OnLogin()
    {
        string user = loginUsername.text;

        if (string.IsNullOrEmpty(user))
        {
            loginMessage.text = "Please enter your name";
            return;
        }

        // ✅ เก็บชื่อผู้เล่น
        PlayerPrefs.SetString("currentUser", user);
        PlayerPrefs.Save();

        loginMessage.text = "Welcome " + user + "!";

        // ไปหน้าเกม
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}