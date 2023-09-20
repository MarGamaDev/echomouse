using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] TriggerScript triggerScript;
    [SerializeField] private bool canBeActivatedByCat;
    [SerializeField] private bool canBeActivatedByPlayer;


    private void OnTriggerEnter(Collider other)
    {
        if (canBeActivatedByPlayer == true && other.CompareTag("Player"))
        {
            triggerScript.Activate();
        }
        else if (canBeActivatedByCat == true && other.CompareTag("Cat"))
        {
            triggerScript.Activate();
        }
    }
}
