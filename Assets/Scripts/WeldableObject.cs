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
    private PhysicsGrabbable physicsGrabbable;
    private TouchHandGrabInteractable touchGrab;
    private ObjectRespawn objectRespawn;

    public bool isAttached;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        physicsGrabbable = GetComponent<PhysicsGrabbable>();
        touchGrab = GetComponent<TouchHandGrabInteractable>();
        objectRespawn = GetComponent<ObjectRespawn>();
    }

    public void TurnOffComponents()
    {
        isAttached = true;

        grabbable.enabled = false;
        physicsGrabbable.enabled = false;
        touchGrab.enabled = false;
        objectRespawn.enabled = false;
        Destroy(rg);
    }

    public void TurnOnComponents()
    {
        isAttached = false;

        grabbable.enabled = true;
        physicsGrabbable.enabled = true;
        touchGrab.enabled = true;
        objectRespawn.enabled = true;

        // Only add a Rigidbody if one doesn't already exist
        if (GetComponent<Rigidbody>() == null)
            rg = gameObject.AddComponent<Rigidbody>();

        GetComponent<PhysicsGrabbable>()._rigidbody = rg;
        rg.mass = 0.1f;
    }
}
