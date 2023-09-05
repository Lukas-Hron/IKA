using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plugg : MonoBehaviour
{
    public bool isGlued;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GlueCollider"))
        {
            isGlued = true;
        }
    }
}
