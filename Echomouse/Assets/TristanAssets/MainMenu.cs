using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevelOne()
    {
        SceneManager.LoadSceneAsync("Blockout");
    }

    public void LoadVictoryScreen()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
