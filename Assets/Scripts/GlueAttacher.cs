using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueAttacher : MonoBehaviour
{

    public GameObject clusterPrefab;
    //[SerializeField] LayerMask weldableObjects;
    public ParticleSpawner parentParticles;
    public SoundPlayer parentSound;

    [SerializeField]
    private InteractableUnityEventWrapper interactEvents;
    [SerializeField]
    private List<GameObject> allColliders = new List<GameObject>();


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            CheckIntersections();
        }


    }

    private void Start()
    {
        interactEvents = GetComponent<InteractableUnityEventWrapper>();

        interactEvents.WhenUnselect.AddListener(CheckIntersections);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpables"))
        {
                allColliders.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUpables"))
        {
            if (allColliders.Contains(other.gameObject))
                allColliders.Remove(other.gameObject);
        }
    }

    public void CheckIntersections()
    {

        Debug.Log("RELEASED");
        if (allColliders.Count > 0)
        {
            if (allColliders[0].GetComponent<WeldableObject>() != null)
            {
                try
                {
                    AttachToObject(this.gameObject, allColliders[0]);
                }
                catch
                {
                }
                return;
            }
            else if(allColliders[0].transform.parent.gameObject.GetComponent<WeldableObject>() != null)
                AttachToObject(this.gameObject, allColliders[0].transform.parent.gameObject);
        }
    }

    public void AttachToObject(GameObject object1, GameObject object2)
    {

        Cluster obj1Cluster = object1.transform.parent?.GetComponent<Cluster>();
        Cluster obj2Cluster = object2.transform.parent?.GetComponent<Cluster>();

        GlueAttacher obj2Glue = object2.transform?.GetComponent<GlueAttacher>();

        if (obj2Glue != null)
        {
            //print("Eyo!?!??!");

            //GetComponent<Rigidbody>().AddForce((object1.transform.position - object2.transform.position).normalized * 100f, ForceMode.Impulse);
            obj2Glue.RemoveGlue();
            //return;
        }

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

            Debug.Log(object1 + " " + object2);
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

        RemoveGlue();



        //if(parentParticles != null)
        parentParticles.PlayBothParticles(transform.position, 0, 0);
        parentParticles.PlayOneParticles(transform.position, 1, false);
        parentSound.PlayAudio(1, transform.position);
        // https://discussions.unity.com/t/how-to-find-the-point-of-contact-with-the-function-ontriggerenter/13338/5

        //soundScript.PlayAudio(2);

    }




    private void RemoveGlue()
    {
        Destroy(this.GetComponent<GlueAttacher>());
    }


}
