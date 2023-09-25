using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] TouchHandGrabInteractable touchGrab;

    public List<GameObject> parts = new List<GameObject>();
    private int amountOfWheels;
    private bool isCar = false;
    public void AddPartToList(GameObject part)
    {
        if (parts.Contains(part)) return;

        part.transform.parent = transform;

        WeldableObject partWeld = part.GetComponent<WeldableObject>();
        switch (partWeld.MyPart)
        {
            case (WeldableObject.Parts.Ordinary): // do nothing
                break;

            case(WeldableObject.Parts.Wheel):
                amountOfWheels++;
                IAmACar();
                break;

            default:
                break;
        }

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

        WeldableObject partWeld = part.GetComponent<WeldableObject>();
        switch (partWeld.MyPart)
        {
            case (WeldableObject.Parts.Ordinary): // do nothing
                break;

            case (WeldableObject.Parts.Wheel):
                amountOfWheels--;
                IAmACar();
                break;

            default:
                break;
        }

        partWeld.TurnOnComponents();


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
    public void IAmACar()
    {
        if(amountOfWheels > 3 && isCar == false) //we arre now a car
        {
            Destroy(GetComponent<ObjectRespawn>());
            gameObject.AddComponent<CarsDoMove>();
        }
        else if(amountOfWheels < 4 && isCar == true) // if we remove a wheel and we no longer should be considered a car
        {
            isCar = false;
            Destroy(GetComponent<CarsDoMove>());
            gameObject.AddComponent<ObjectRespawn>();
        }
    }

}
