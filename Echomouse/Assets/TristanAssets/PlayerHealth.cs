using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private float playerHealth;

    private void Update()
    {
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            playerHealth -= 1;
        }
    }
}
