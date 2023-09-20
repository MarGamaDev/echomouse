using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private bool canActivate = true;
    [SerializeField] private float activationCoolDown;
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if (!canActivate)
        {
            return;
        }

        onActivated?.Invoke();
        audioSource.PlayOneShot(audioClip);
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(SetCanActivateFalse());
        }
    }

    private IEnumerator SetCanActivate()
    {
        yield return null;
        canActivate = true;
        currentCoroutine = null;
    }

    private IEnumerator SetCanActivateFalse()
    {
        yield return null;
        canActivate = false;
        yield return new WaitForSeconds(activationCoolDown);
        currentCoroutine = null;
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(SetCanActivate());
        }
    }

}
