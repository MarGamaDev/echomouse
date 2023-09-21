using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyClapper : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource aSor;
    [SerializeField] private AudioClip aClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Clap()
    {
        StartCoroutine(ClapRoutine());
    }

    private IEnumerator ClapRoutine()
    {
        animator.SetTrigger("Clap");
        aSor.PlayOneShot(aClip);
        while (aSor.isPlaying)
        {
            yield return null;
        }
        animator.SetTrigger("Unclap");
    }
}
