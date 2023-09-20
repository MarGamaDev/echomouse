using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] TriggerScript triggerScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerScript.Activate();
        }
    }
}
