using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] TouchHandGrabInteractable touchGrab;

    public List<GameObject> parts = new List<GameObject>();

    public void AddPartToList(GameObject part)
    {
        if (parts.Contains(part)) return;

        part.transform.parent = transform;
        part.GetComponent<WeldableObject>().TurnOffComponents();

        parts.Add(part);

        Collider partColl = part.GetComponent<Collider>();
        if (partColl != null)
            touchGrab._colliders.Add(partColl);
    }

    public void RemovePartFromList(GameObject part)
    {
        if (!parts.Contains(part)) return;

        part.transform.parent = null;
        part.GetComponent<WeldableObject>().TurnOnComponents();

        parts.Remove(part);

        touchGrab._colliders.Remove(part.GetComponent<Collider>());

        // if this cluster is empty Destroy
        if (parts.Count <= 0)
            Destroy(this.gameObject);
    }

    public void AddThisClusterToAnotherOne(Cluster clusterToAddTo)
    {
        foreach (GameObject part in parts)
        {
            RemovePartFromList(part);

            clusterToAddTo.AddPartToList(part);
        }
    }
}
