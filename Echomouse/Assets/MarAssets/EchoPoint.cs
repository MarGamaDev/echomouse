using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EchoPoint : MonoBehaviour
{
    [SerializeField] private float animateLengthInSeconds = 1f;
    [SerializeField] private float timeMult = 1f;
    [SerializeField] private Material material;

    private void Start()
    {
        StartPulse();
    }

    public void StartPulse()
    {
        material.SetVector("_Origin", transform.position);
        material.SetFloat("_Timer", 0);
        StartCoroutine(CountDownToDemise());
    }

    private IEnumerator CountDownToDemise()
    {
        float percent = 0;
        while (percent < 1)
        {
            yield return null;
            material.SetFloat("_Timer", percent * timeMult);
            percent += Time.deltaTime / animateLengthInSeconds;
        }

        EchoPointManager.instance.ReturnPulse(gameObject);
    }

    private void LateUpdate()
    {
        material.SetVector("_Origin", transform.position);
    }
}
