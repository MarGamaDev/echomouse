using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivated;
    [SerializeField]private bool canActivate = true;
    private Coroutine currentCoroutine;

    public void Activate()
    {
        if (!canActivate)
        {
            return;
        }

        onActivated?.Invoke();
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
        yield return new WaitForSeconds(2f);
        currentCoroutine = null;
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(SetCanActivate());
        }
    }

}
