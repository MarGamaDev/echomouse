using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoPointManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();

    public static EchoPointManager instance;

    private void Start()
    {
        instance = this;
    }

    public void GetPulse(Vector3 position)
    {
        if (points.Count > 0)
        {
            points[points.Count - 1].transform.position = position;
            points[points.Count - 1].GetComponent<EchoPoint>().StartPulse();
            points.RemoveAt(points.Count - 1);
        }
    }

    public void ReturnPulse(GameObject pulseReference)
    {
        pulseReference.transform.position = transform.position;
        points.Add(pulseReference);
    }
}
