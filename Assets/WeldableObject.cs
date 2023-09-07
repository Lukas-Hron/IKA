using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldableObject : MonoBehaviour
{
    private Rigidbody rg;
    public bool isAttached;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }


    public void AttachToObject(GameObject targetObject, bool recursiveCall = false)
    {
        WeldableObject weldable = targetObject.GetComponent<WeldableObject>();

        // Check to make sure the object has a weldable object script on it
        if (weldable == null)
        {
            Debug.Log("WeldableObject component not found!");
            return;
        }

        RemoveRigidbody();
        isAttached = true;
        if (weldable.isAttached)
        {
            gameObject.transform.SetParent(weldable.transform.parent);
        }
        else
        {
            // Create a new empty GameObject called "Cluster" at the current GameObject's position
            GameObject cluster = new GameObject("Cluster");
            cluster.AddComponent<Rigidbody>();
            //cluster.transform.position = this.transform.position;

            // Set the parent of the current GameObject and the weldable object to the cluster
            this.transform.SetParent(cluster.transform);

            // Only attach back if it's not a recursive call
            if (!recursiveCall)
            {
                weldable.AttachToObject(gameObject, true);
            }
        }
    }


    public void RemoveFromObject()
    {
        if (!isAttached)
        {
            Debug.Log("Object was already not attached!");
            return;
        }

        isAttached = false;


        if (this.transform.parent != null)
        {
            this.transform.SetParent(null);
        }

        GameObject paren = transform.parent.gameObject;
        if (paren.transform.childCount == 0)
        {
            Destroy(paren);
        }


        // Only add a Rigidbody if one doesn't already exist
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            rg = gameObject.AddComponent<Rigidbody>();
        }
    }


    public void RemoveRigidbody()
    {
        Destroy(rg);
    }
}
