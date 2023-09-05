using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeededForWeld : MonoBehaviour
{
    private WeldableObject weldableObjectScript;

    //objects that can and will need to be welded to this object
    [SerializeField] List<WeldableObject> weldObjects = new List<WeldableObject>();

    public string itemID;
    public string connectionID;

    private void Start()
    {
        weldableObjectScript = GetComponent<WeldableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
