using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingWand : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WeldableObject>() != null)
        {
            if (other.GetComponent<WeldableObject>().isAttached)
            {
                other.GetComponentInParent<Cluster>().RemovePartFromList(other.gameObject);
            }
        }
    }
}
