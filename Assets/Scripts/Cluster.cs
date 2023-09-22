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

        WeldableObject partWeld = part.GetComponent<WeldableObject>();
        partWeld.TurnOffComponents();
        partWeld.ChangeInteractableEventHandler(GetComponent<TouchHandGrabInteractable>());

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
        {
            Destroy(this.gameObject);
        }
    }

    public void AddThisClusterToAnotherOne(Cluster clusterToAddTo)
    {
        int g = parts.Count;

        for (int i = 0; i < g; i++)
        {
            clusterToAddTo.AddPartToList(parts[i]);
            parts.Remove(parts[i]);
        }

        Destroy(this.gameObject);
    }
}
