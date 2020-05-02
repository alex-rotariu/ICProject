using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadGameScene() {
        ScenesData.SetPolitician(FindObjectOfType<SwipeMenu>().getSelection()); // setting the chosen politician from game menu to the game scene
        SceneManager.LoadScene("Game");
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
}
