using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCheck : MonoBehaviour
{
    public void StartCheck()
    {
        foreach(CheckForPoints child in transform.GetComponentsInChildren<CheckForPoints>())
        {
            child.CheckPoints();
        }
    }

}
