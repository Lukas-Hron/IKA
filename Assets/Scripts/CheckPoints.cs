using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] PointCounter point;
    public void StartCheck()
    {
        foreach(PointChecker child in transform.GetComponentsInChildren<PointChecker>())
        {
            point.points = 0;
            child.CheckPoints();
        }
    }
}
