using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPing : MonoBehaviour
{
    [SerializeField] private Transform originOverride;

    public void Ping()
    {
        Vector3 pos = transform.position;
        if (originOverride != null)
            pos = originOverride.position;
        EchoPointManager.instance.GetPulse(pos);
    }
}
