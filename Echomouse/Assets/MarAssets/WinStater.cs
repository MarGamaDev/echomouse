using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinStater : MonoBehaviour
{
    [SerializeField] private GameObject winText, loseText;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private PauseMenu pauseMenu;

    public void Lose()
    {
        Time.timeScale = 0;
        pauseMenu.CanPause= false;
        loseText.SetActive(true);
        exitButton.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0;
        pauseMenu.CanPause = false;
        winText.SetActive(true);
        exitButton.SetActive(true);
    }
}
