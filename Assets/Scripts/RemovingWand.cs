using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingWand : MonoBehaviour
{
    private bool isGrabbed;
    public void SetIsGrabbedBool(bool value) { isGrabbed = value; }


    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbed) return;

        if (other.GetComponent<WeldableObject>() != null)
        {
            if (other.GetComponent<WeldableObject>().isAttached)
            {
                other.GetComponentInParent<Cluster>().RemovePartFromList(other.gameObject);
            }
        }
    }
}
