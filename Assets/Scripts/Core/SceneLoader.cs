using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadGameSceneLoggedIn() {
        SceneManager.LoadScene("Game");
    }

    public void loadGameScene()
    {
        int index = FindObjectOfType<SwipeMenu>().getSelection();
        ScenesData.SetCharacter(index);
        loadGameSceneLoggedIn();
    }

    public void loadRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }

    public void loadLogInScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void loadSelectionScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void loadLeaderboard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
