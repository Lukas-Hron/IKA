using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluggHole : MonoBehaviour
{
    public Transform pluggPosition;
    public Transform pluggDirection;

    [SerializeField] GameObject plugg;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == plugg)
        {
            ConnectPlugg();
        }
    }

    private void ConnectPlugg()
    {
        
    }
}
