using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveEcho : MonoBehaviour
{
    [SerializeField] private Material material;
    private float timer = 0f;


    private void LateUpdate()
    {
        material.SetVector("_Origin", transform.position);
        material.SetFloat("_Timer", timer);
        timer += Time.deltaTime;
    }
}
