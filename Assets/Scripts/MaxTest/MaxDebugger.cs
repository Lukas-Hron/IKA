using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    public List <WeldableObject> WeldObjects;
    public HandVisual hand;
    public GameObject clusterPrefab;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            for (int i = 1; i < WeldObjects.Count; i++)
            {
                AttachToObject(WeldObjects[0].gameObject, WeldObjects[i].gameObject);
            }
        }
        
    }
    public void AttachToObject(GameObject object1, GameObject object2)
    {

        Cluster obj1Cluster = object1.transform.parent?.GetComponent<Cluster>();
        Cluster obj2Cluster = object2.transform.parent?.GetComponent<Cluster>();

       // GlueAttacher obj2Glue = object2.transform?.GetComponent<GlueAttacher>();

       

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
        // https://discussions.unity.com/t/how-to-find-the-point-of-contact-with-the-function-ontriggerenter/13338/5

        //soundScript.PlayAudio(2);

    }
    
}
