using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadSceneAsync(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
