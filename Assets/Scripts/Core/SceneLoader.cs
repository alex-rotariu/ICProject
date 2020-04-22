using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadGameScene() {
        ScenesData.SetPolitician(FindObjectOfType<SwipeMenu>().getSelection()); // setting the chosen politician from game menu to the game scene
        SceneManager.LoadScene(1);
    }
}
