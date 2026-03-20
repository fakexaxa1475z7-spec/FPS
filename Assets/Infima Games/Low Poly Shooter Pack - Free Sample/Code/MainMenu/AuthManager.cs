using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Login UI")]
    public TMP_InputField loginUsername;
    public TMP_InputField loginPassword;
    public TextMeshProUGUI loginMessage;

    [Header("Register UI")]
    public TMP_InputField registerUsername;
    public TMP_InputField registerPassword;
    public TMP_InputField confirmPassword;
    public TextMeshProUGUI registerMessage;

    // 🔐 Register
    public void OnRegister()
    {
        string user = registerUsername.text;
        string pass = registerPassword.text;
        string confirm = confirmPassword.text;

        if (user == "" || pass == "")
        {
            registerMessage.text = "Please fill in all the required information";
            return;
        }

        if (pass != confirm)
        {
            registerMessage.text = "The passwords don't match";
            return;
        }

        if (PlayerPrefs.HasKey(user))
        {
            registerMessage.text = "This username is already in use";
            return;
        }

        PlayerPrefs.SetString(user, pass);
        PlayerPrefs.Save();

        registerMessage.text = "Registration successful!";
    }

    // 🔑 Login
    public void OnLogin()
    {
        string user = loginUsername.text;
        string pass = loginPassword.text;

        if (!PlayerPrefs.HasKey(user))
        {
            loginMessage.text = "Username not found";
            return;
        }

        string savedPass = PlayerPrefs.GetString(user);

        if (pass == savedPass)
        {
            loginMessage.text = "Login successful!";

            // ✅ เก็บ user ที่ login
            PlayerPrefs.SetString("currentUser", user);
            PlayerPrefs.Save();
            // ไปหน้าเกม
            SceneManager.LoadScene("Game");
        }
        else
        {
            loginMessage.text = "The password is incorrect";
        }
    }
    public void Logout()
    {
        PlayerPrefs.DeleteKey("currentUser");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}