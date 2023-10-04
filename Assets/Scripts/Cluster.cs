using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] TouchHandGrabInteractable touchGrab;

    public List<GameObject> parts = new List<GameObject>();
    private int amountOfWheels;
    private bool isCar = false;

    private int amountShelves;
    private bool isShelf = false;

    private Vector3 midPosition;

    
    public void AddPartToList(GameObject part)
    {
        if (parts.Contains(part)) return;

        part.transform.parent = transform;


        WeldableObject partWeld = part.GetComponent<WeldableObject>();
        switch (partWeld.MyPart)
        {
            case (WeldableObject.Parts.Ordinary): // do nothing
                break;

            case (WeldableObject.Parts.Wheel):
                amountOfWheels++;
                IAmACar();
                break;

            case (WeldableObject.Parts.Anchor):
                amountShelves++;
                IAmAShelf();
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
        else
            Debug.Log("Missing Collider");

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
            case (WeldableObject.Parts.Anchor):
                amountShelves--;
                IAmAShelf();
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

    public void RemoveAllPartsFromList()
    {

        CalculateMiddlePosOfChildren();

        for (int i = parts.Count; i > 0; i--)
        {
            int index = i - 1;
            parts[index].transform.parent = null;

            WeldableObject partWeld = parts[index].GetComponent<WeldableObject>();
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


            touchGrab._colliders.Remove(parts[index].GetComponent<Collider>());




            parts[index].GetComponent<Rigidbody>().AddForce((parts[index].transform.position - midPosition) * 5,ForceMode.Impulse);
            parts.RemoveAt(index);
        }
        
        Destroy(gameObject);

    }

    void CalculateMiddlePosOfChildren()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            midPosition += parts[i].transform.position;
        }

        midPosition /= parts.Count;
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
            isCar = true;
        }
        else if(amountOfWheels < 4 && isCar == true) // if we remove a wheel and we no longer should be considered a car
        {
            isCar = false;
            Destroy(GetComponent<CarsDoMove>());
            gameObject.AddComponent<ObjectRespawn>();
        }
    }
    private void IAmAShelf()
    {
        if (amountShelves > 1) 
            isShelf = true;

        if (amountShelves < 2)
            isShelf = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForWallCollision(collision);
    }

    private void CheckForWallCollision(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;
        if (isShelf == false) return;

        Destroy(gameObject.GetComponent<ObjectRespawn>());
        Destroy(gameObject.GetComponent<TouchHandGrabInteractable>());
        Destroy(gameObject.GetComponent<PhysicsGrabbable>());
        Destroy(gameObject.GetComponent<Grabbable>());
        Destroy(GetComponent<Rigidbody>());

        foreach (Transform child in transform)
        {
            Destroy(child.GetComponent<WeldableObject>());
        }

        gameObject.tag = "Wall";

        Destroy(this);
    }

    public void Selected()
    {
        if(isCar == true)
        {
            GetComponent<CarsDoMove>().StartCar(true);
            Debug.Log("Pickued up car");
        }
    }
}
