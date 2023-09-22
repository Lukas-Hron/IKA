using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] GameObject clusterPrefab;

    [SerializeField] LayerMask weldableObjects;

    private bool isGrabbed;
    public void SetIsGrabbedBool(bool value) => isGrabbed = value;

    private ParticleSpawner particleScript;
    private SoundPlayer soundScript;
    private Vector3 meldPosition;

    private void Start()
    {
        particleScript = GetComponent<ParticleSpawner>();
        soundScript = GetComponent<SoundPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbed) return;
        // Check if the entered object has the tag "PickUpable"
        if (other.CompareTag("PickUpables"))
        {
            soundScript.PlayAudio(0);
            // Check if the object has the "WeldableObject" component
            WeldableObject weldable = other.GetComponent<WeldableObject>();
            if (weldable != null)
            {
                ShootRay(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isGrabbed) return;
        // Check if the entered object has the tag "PickUpable"
        if (other.CompareTag("PickUpables"))
        {
            soundScript.PlayAudio(1);
        }
    }


    void ShootRay(GameObject other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, weldableObjects))
        {

            particleScript.PlayAllParticles(hit.point, 1, 0, true);


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
                        meldPosition = hitInfo.point;
                        break;  // Exit the loop once found
                    }
                }
            }

            if (targetObject != null)
            {
                AttachToObject(hit.collider.gameObject, targetObject);
                //Debug.Log("Welding " + hit.collider.gameObject.name + " to " + targetObject.name + ".");


            }
            else
            {
                Debug.Log("Failed to find a weldable object");
            }
        }
    }

    public void AttachToObject(GameObject object1, GameObject object2)
    {
        Cluster obj1Cluster = object1.transform.parent?.GetComponent<Cluster>();
        Cluster obj2Cluster = object2.transform.parent?.GetComponent<Cluster>();

        // both objects are in clusters
        if (obj1Cluster && obj2Cluster)
        {
            // they're in same cluster
            if (obj1Cluster == obj2Cluster) return;

            obj1Cluster.AddThisClusterToAnotherOne(obj2Cluster);
        }

        // neither object is in a cluster so create one
        else if (!obj1Cluster && !obj2Cluster)
        {
            GameObject newCluster = Instantiate(clusterPrefab, object1.transform.position, Quaternion.identity);

            newCluster.GetComponent<Cluster>().AddPartToList(object1);
            newCluster.GetComponent<Cluster>().AddPartToList(object2);

            object1.transform.SetParent(newCluster.transform);
            object2.transform.SetParent(newCluster.transform);
        }
        else // one of them is in a cluster, check which one and add the other object to it
        {
            if (obj1Cluster)
            {
                obj1Cluster.AddPartToList(object2);
            }
            else if (obj2Cluster)
            {
                obj2Cluster.AddPartToList(object1);
            }
        }

        soundScript.PlayAudio(2);
        particleScript.PlayAllParticles(meldPosition,0,0);

    }
}

