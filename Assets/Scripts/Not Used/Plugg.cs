using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plugg : MonoBehaviour
{
    [SerializeField] Fasteners pluggSO;
    public bool isGlued;
    public bool isHeld;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GlueCollider"))
        {
            isGlued = true;
        }
    }
}
