using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEditor;
using UnityEngine;

public class WeldableObject : MonoBehaviour
{
    private Rigidbody rg;
    private Grabbable grabbable;
    private HandGrabInteractable HandGrabInteractable;
    private PhysicsGrabbable physicsGrabbable;

    public bool isAttached;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        HandGrabInteractable = GetComponent<HandGrabInteractable>();
        physicsGrabbable = GetComponent<PhysicsGrabbable>();
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
            //GameObject newCluster = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Cluster.prefab"));
            GameObject newCluster = Instantiate(new GameObject());
            
            newCluster.AddComponent<Rigidbody>();
            newCluster.AddComponent<Grabbable>();
            newCluster.AddComponent<HandGrabInteractable>();
            newCluster.AddComponent<PhysicsGrabbable>();
            
            newCluster.GetComponent<PhysicsGrabbable>()._rigidbody = newCluster.GetComponent<Rigidbody>();
            newCluster.GetComponent<PhysicsGrabbable>()._grabbable = newCluster.GetComponent<Grabbable>();

            newCluster.GetComponent<HandGrabInteractable>()._rigidbody = newCluster.GetComponent<Rigidbody>();

newCluster.transform.position = this.transform.position;
            this.transform.SetParent(newCluster.transform);

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
            grabbable.enabled = true;
            HandGrabInteractable.enabled = true;
            physicsGrabbable.enabled = true;
        }
    }


    public void RemoveRigidbody()
    {
        grabbable.enabled = false;
        HandGrabInteractable.enabled = false;
        physicsGrabbable.enabled = false;
        Destroy(rg);
    }
}
