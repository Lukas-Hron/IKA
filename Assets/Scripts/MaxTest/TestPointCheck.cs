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
    Mesh mesh;
    void Start()
    {
        collider = GetComponent<Collider>();
        mesh = GetComponent<Mesh>();
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

        overlappingGameObjects?.Add(other.transform.gameObject);
    }

    private void CheckColliders()
    {
        GameObject parent = null;
        for(int i = 0; i < overlappingGameObjects.Count; i++)
        {
            if (overlappingGameObjects[i].GetComponent<Mesh>() == mesh)
            {
                parent = overlappingGameObjects[i];
                break; // if we found the right mesh
            }
            if(i < overlappingGameObjects.Count - 1 && parent == null)
            {
                sendScoreInfo.Invoke(0, 0, false);
                return;
            }
        }

        if (parent == null)
            return;

        Debug.Log(parent.ToString());
        Transform capsules = parent.transform.GetChild(0);

        GameObject holder = capsules.gameObject;//gameobject that holds gameobjects with capsules
       

        if (holder == null) //if the mesh was right but still had no colliders in it something is seriously wrong
        {
            Debug.LogError("Mesh of part" + holder.transform.parent.name.ToString() + " is right but has no CapsuleHolder");
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
