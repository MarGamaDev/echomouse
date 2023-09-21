using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private float playerHealth;
    [SerializeField] private AudioClip smackClip;
    AudioSource smackSource;
    private WinStater winStater;

    private void Awake()
    {
        winStater = FindObjectOfType<WinStater>();
    }

    private void Update()
    {
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        winStater.Lose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            playerHealth -= 1;
            smackSource.PlayOneShot(smackClip);
        }
    }
}
