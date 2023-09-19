using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoMasker : MonoBehaviour
{
    [SerializeField] private float minScale = 0.1f, maxScale = 1f;
    [SerializeField] private float animateLengthInSeconds = 1f;
    [SerializeField] private AnimationCurve pulseCurve = new AnimationCurve();

    private void Start()
    {
        StartCoroutine(PulseOverTime());
    }

    private IEnumerator PulseOverTime()
    {
        float percent = 0;
        while (percent < 1)
        {
            float curvePercent = pulseCurve.Evaluate(percent);
            float remappedSize = Mathf.Lerp(minScale, maxScale, curvePercent);
            transform.localScale = new(remappedSize, remappedSize, remappedSize);
            yield return null;
            percent += Time.deltaTime / animateLengthInSeconds;
        }

        Destroy(gameObject);
    }
}
