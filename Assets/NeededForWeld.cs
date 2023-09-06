using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeededForWeld : MonoBehaviour
{
    private WeldableObject weldableObjectScript;

    // objects that can and will need to be welded to this object, like other planks
    [SerializeField] List<WeldableObject> weldObjects = new List<WeldableObject>();

    // this items id for pdf reference
    public string itemID = "A1";

    // needed to know which object should be connected to this object, 
    public string pluggConnectionID = "0";
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
        Debug.Log("added plugg" + pluggHole.gameObject.name);
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
        Debug.Log(currentConnectionID);

        // all plugs are correct to connect to next piece
        if (currentConnectionID == pluggConnectionID)
        {
            canConnectToNextPiece = true;
            Debug.Log("Can connect");
        }
    }
}
