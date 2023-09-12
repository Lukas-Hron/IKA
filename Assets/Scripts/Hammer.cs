using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] GameObject clusterPrefab;

    [SerializeField] LayerMask weldableObjects;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the tag "PickUpable"
        if (other.CompareTag("PickUpables"))
        {
            // Check if the object has the "WeldableObject" component
            WeldableObject weldable = other.GetComponent<WeldableObject>();
            if (weldable != null)
            {
                ShootRay(other.gameObject);
            }
        }
    }

    void ShootRay(GameObject other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, weldableObjects))
        {
            // Shoot another ray from the hit point in the direction of the normal
            Ray secondRay = new Ray(hit.point, hit.normal * -1);
            RaycastHit secondHit;
            Physics.Raycast(secondRay, out secondHit, 100f, weldableObjects);

            RaycastHit[] hits = Physics.RaycastAll(secondRay, 100f);
            GameObject targetObject = null;

            foreach (var hitInfo in hits)
            {
                if (hitInfo.collider.gameObject != hit.collider.gameObject)
                {
                    WeldableObject weldable = hitInfo.collider.gameObject.GetComponent<WeldableObject>();
                    if (weldable != null)
                    {
                        targetObject = hitInfo.collider.gameObject;
                        break;  // Exit the loop once found
                    }
                }
            }

            if (targetObject != null)
            {
                AttachToObject(hit.collider.gameObject, targetObject);
                Debug.Log("Welding " + hit.collider.gameObject.name + " to " + targetObject.name + ".");
            }
            else
            {
                Debug.Log("Failed to find a weldable object");
            }
        }
    }

    public void AttachToObject(GameObject object1, GameObject object2)
    {
        Cluster obj1Cluster = null;
        Cluster obj2Cluster = null;
        try
        {
            obj1Cluster = object1.transform.parent.GetComponent<Cluster>();
            obj2Cluster = object2.transform.parent.GetComponent<Cluster>();

        }
        catch (System.Exception)
        {

        }

        // both objects are in the same cluster
        if (obj1Cluster && obj2Cluster && object1.transform.parent == object2.transform.parent) return;
       
        // neither object is in a cluster so create one
        if (!obj1Cluster && !obj2Cluster)
        {
            GameObject newCluster = Instantiate(clusterPrefab, object1.transform.position, Quaternion.identity);

            newCluster.GetComponent<Cluster>().AddPartToList(object1);
            newCluster.GetComponent<Cluster>().AddPartToList(object2);

            object1.transform.SetParent(newCluster.transform);
            object2.transform.SetParent(newCluster.transform);
        }
        else // check which object has cluster and add the other object to it
        {
            if (obj1Cluster)
            {
                obj1Cluster.AddPartToList(object2);
                Debug.Log("add to cluster " + obj1Cluster.name);
            }
            else if (obj2Cluster)
            {
                obj2Cluster.AddPartToList(object1);
                Debug.Log("add to cluster " + obj2Cluster.name);
            }
        }
    }
}

