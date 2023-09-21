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

    private void Start()
    {
        Invoke("Lose", 3f);
    }

    public void Lose()
    {
        loseText.SetActive(true);
        exitButton.SetActive(true);
    }

    public void Win()
    {
        winText.SetActive(true);
        exitButton.SetActive(true);
    }
}
