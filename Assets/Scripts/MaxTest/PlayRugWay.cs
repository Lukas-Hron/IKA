using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRugWay : MonoBehaviour
{
    public List<Transform> wayPoints = new();
    public Action<float> Drive;

    
    private void Start()
    {
        foreach (Transform child in transform) // create a track
        {
            wayPoints.Add(child);
        }
    }

    void Update()
    {
        Drive?.Invoke(Time.deltaTime);
    }
}
