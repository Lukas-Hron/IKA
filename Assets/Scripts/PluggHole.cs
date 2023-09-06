using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PluggHole : MonoBehaviour
{
    public bool hasPlugg;
    public bool needsGlue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plugg>() != null) //if other is plugg
        {
            if (needsGlue && other.gameObject.GetComponent<Plugg>().isGlued)//if plugg is glued and should be glued - connect
                ConnectPlugg(other.gameObject);

            else if (!needsGlue && !other.gameObject.GetComponent<Plugg>().isGlued)//reversed
                ConnectPlugg(other.gameObject);

            else
            {
                //pop away function that probably should be in weldableobject later
            }
        }
    }

    public void ConnectPlugg(GameObject plugg)
    {
        //cant connect if plugg is already connected
        if (hasPlugg || plugg.transform.parent != null) return;

        Debug.Log("hej igen" + gameObject.name);
        hasPlugg = true;

        plugg.transform.position = transform.position;
        plugg.transform.rotation = transform.rotation;
        plugg.transform.parent = transform;

        var rb = plugg.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        if (transform.parent != null)
            GetComponentInParent<NeededForWeld>().AddedPlugg(this);
    }

    public void RemovePlugg(GameObject plugg)
    {
        if (!hasPlugg) return;

        hasPlugg = false;

        plugg.transform.parent = null;

        var rb = plugg.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        if (transform.parent != null)
            GetComponentInParent<NeededForWeld>().RemovedPlugg(this);
    }
}

