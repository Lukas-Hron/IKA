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
    private List<Collider> otherColliders = new List<Collider>();
    private List<GameObject> overlappingGameObjects = new List<GameObject>();

    int overlappingColliders = 0;
    int amountColliders = 0;
    MeshFilter mesh;
    void Start()
    {
        collider = GetComponent<Collider>();
        mesh = GetComponent<MeshFilter>();
        Debug.LogWarning(mesh.mesh.ToString());
    }

    // Update is called once per frame
    private void OnEnable()
    {
        ScoreManager.TestingAttentionPlease += CheckColliders;
    }
    private void OnDisable()
    {
        ScoreManager.TestingAttentionPlease -= CheckColliders;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (overlappingGameObjects.Contains(other?.transform.gameObject))
            return;

        if (other.gameObject.transform.parent.CompareTag("CapsuleHolder"))
        {
            overlappingGameObjects?.Add(other.transform.parent.transform.parent.gameObject); 
            return;
        }

        if (other.GetComponent<MeshFilter>() == null)
            return;
        overlappingGameObjects?.Add(other.transform.gameObject);
    }

    private void CheckColliders()
    {
        GameObject partOfFurniture = null;
        for (int i = 0; i < overlappingGameObjects.Count; i++)
        {
            Debug.Log(overlappingGameObjects[i].GetComponent<MeshFilter>().mesh.ToString());
            if (overlappingGameObjects[i].GetComponent<MeshFilter>().mesh.ToString() == mesh.mesh.ToString())
            {
                partOfFurniture = overlappingGameObjects[i];
               
                break; // if we found the right mesh
            }
            
        }

        if (partOfFurniture == null)
        {
            if (partOfFurniture == null)
            {
                sendScoreInfo.Invoke(0, 0, false); // could be moved down one

                return;
            }
        }
        GameObject holder = null;
        if (partOfFurniture.transform.childCount > 0 && partOfFurniture.transform.GetChild(0).CompareTag("CapsuleHolder"))
        {
            holder = partOfFurniture.transform.GetChild(0).gameObject;
        }
         //the first child of each object needs to have the capsules

        //gameobject that holds gameobjects with capsules
       

        if (holder == null) //if the mesh was right but still had no colliders in it something is seriously wrong
        {
            Debug.LogError("Mesh of part" + partOfFurniture.name.ToString() + " is right but has no CapsuleHolder");
            return;
        }

        otherColliders = holder.GetComponentsInChildren<Collider>().ToList<Collider>();

        for (int i = 0; i < otherColliders.Count; i++)
        {
            if (collider.bounds.Intersects(otherColliders[i].bounds))
            {
                overlappingColliders++;
            }
            amountColliders++;
        }

        //send the amount of colliders that are overlapping and the amount of colliders within teh capsule holder
        sendScoreInfo.Invoke(overlappingColliders, amountColliders, true);
    }
    
}
