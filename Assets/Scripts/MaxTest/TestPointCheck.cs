using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class TestPointCheck : MonoBehaviour
{
    public static Action <int, int, bool> sendScoreInfo; // ovelapping colliders, amountOfColliders, valid mesh

    Collider collider;
    private List<Collider> otherColliders = new List<Collider>(); // colliders of the part we are checking
    private List<GameObject> overlappingGameObjects = new List<GameObject>(); // all overlapping parts inside the collider radius

    int overlappingColliders = 0; //information that gets sent to the scoremanager
    int amountColliders = 0;

    MeshFilter mesh; // used to check if its the right mesh in the right place
    void Start()
    {
        collider = GetComponent<Collider>();
        mesh = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        ScoreManager.Facit += CheckColliders; // scoremanager calls the delegate in order to triggre the checking process
    }
    private void OnDisable()
    {
        ScoreManager.Facit -= CheckColliders;
    }

    private void OnTriggerEnter(Collider other) // add all collisionn objects into overlapping gameobjects
    {
        if (overlappingGameObjects.Contains(other?.transform.gameObject)) // if the object is already in the list of overlapping objects then skip procedure
            return;

        if (other.gameObject.transform.parent.CompareTag("CapsuleHolder")) // if were colliding with the capsules of the object, then add the object
        {
            overlappingGameObjects?.Add(other.transform.parent.transform.parent.gameObject); 
            return;
        }

        if (other.GetComponent<MeshFilter>() == null) // if it doesnt contain a mesh return
            return;

        overlappingGameObjects?.Add(other.transform.gameObject); // if nothing failed proceed to add into the list of objects
    }

    private void CheckColliders()
    {
        GameObject partOfFurniture = null;

        for (int i = 0; i < overlappingGameObjects.Count; i++)
        {
            if (overlappingGameObjects[i].GetComponent<MeshFilter>().mesh.ToString() == mesh.mesh.ToString()) // string compare of the meshes
            {
                partOfFurniture = overlappingGameObjects[i];
               
                break; // if we found the right mesh
            }
        }

        if (partOfFurniture == null) // if the wrong furniture is overlapping
        {
            sendScoreInfo.Invoke(0, 0, false); 

            return;
        }

        GameObject holder = null;
        if (partOfFurniture.transform.childCount > 0 && partOfFurniture.transform.GetChild(0).CompareTag("CapsuleHolder")) // if the furniture has a child (collider holder) and it is
        {                                                                                                                  // a capsule holder it means weve hit something worth looking at
            holder = partOfFurniture.transform.GetChild(0).gameObject;                                                     //the first child of each object needs to have the capsules
        }
         


        if (holder == null) //if the mesh was right but still had no colliders in it something is seriously wrong
        {
            Debug.LogError("Mesh of part" + partOfFurniture.name.ToString() + " is right but has no CapsuleHolder");
            return;
        }

        otherColliders = holder.GetComponentsInChildren<Collider>().ToList<Collider>(); // list of the colliders inside the furniture used to count points

        for (int i = 0; i < otherColliders.Count; i++)
        {
            if (collider.bounds.Intersects(otherColliders[i].bounds)) //check if the colliders intersect
            {
                overlappingColliders++;
            }
            amountColliders++;
        }

        //send the amount of colliders that are overlapping and the amount of colliders within teh capsule holder
        sendScoreInfo.Invoke(overlappingColliders, amountColliders, true);
    }
    
}
