using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onActivated;
    private bool canActivate = true;

    public void Activate()
    {
        if (!canActivate)
        {
            return;
        }

        onActivated?.Invoke();
    }

    private IEnumerator SetCanActivate()
    {
        canActivate = true;
        yield return null;
    }

}
