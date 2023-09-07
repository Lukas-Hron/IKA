using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeededForWeld : MonoBehaviour
{
    private WeldableObject weldableObjectScript;

    // objects that can and will need to be welded to this object, like other planks
    [SerializeField] WeldableObject nextObjectToWeldTo;

    // this items id for pdf reference
    public string itemID = "A1";

    // needed to know which object should be connected to this object
    // if there are more than one object to be connected add another string to dictionary
    // key is the itemID and value is the connectionID
    public Dictionary<string, string> pluggConnectionID = new Dictionary<string,string>();
    public bool canConnectToNextPiece;

    // how many and in what spots the plugs are, 0 = empty 1 = filled
    private Dictionary<PluggHole, int> connectedPluggHoles = new Dictionary<PluggHole, int>();



    private void Start()
    {
        weldableObjectScript = GetComponent<WeldableObject>();

        PluggHole[] children = transform.GetComponentsInChildren<PluggHole>();
        foreach (PluggHole pluggHole in children)
        {
            connectedPluggHoles.Add(pluggHole, 0);
        }
    }

    public void AddedPlugg(PluggHole pluggHole)
    {
        connectedPluggHoles[pluggHole] = 1;
        CheckConnectionID();
    }

    public void RemovedPlugg(PluggHole pluggHole)
    {
        connectedPluggHoles[(pluggHole)] = 0;
        CheckConnectionID();
    }

    private void CheckConnectionID()
    {
        string currentConnectionID = "";

        foreach (KeyValuePair<PluggHole, int> item in connectedPluggHoles)
        {
            currentConnectionID += connectedPluggHoles[item.Key].ToString();
        }

        // all plugs are correct to connect to next piece
        if (pluggConnectionID.Count == 1)
        {
            //if (currentConnectionID == pluggConnectionID[0])
            //{
            //    canConnectToNextPiece = true;
            //    Debug.Log("Can connect");
            //}
        }
    }
}
