using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InteractableUnityEventWrapper))]



public class WeldableObject : MonoBehaviour
{
    private Rigidbody rg;
    private Grabbable grabbable;
    private InteractableUnityEventWrapper interactEvent;
    private PhysicsGrabbable physicsGrabbable;
    private TouchHandGrabInteractable touchGrab;
    private ObjectRespawn objectRespawn;
    public bool isAttached;
    public enum Parts {Ordinary, Wheel}
    public Parts MyPart = Parts.Ordinary;
    

    void Start()
    {
        interactEvent = GetComponent<InteractableUnityEventWrapper>();
        rg = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
        physicsGrabbable = GetComponent<PhysicsGrabbable>();
        touchGrab = GetComponent<TouchHandGrabInteractable>();
        objectRespawn = GetComponent<ObjectRespawn>();


        if (interactEvent && rg && grabbable && physicsGrabbable && touchGrab && objectRespawn == null)
            Debug.LogError("MISSING COMPONENT IN " + this.gameObject);
    }

    public void TurnOffComponents()
    {
        isAttached = true;

        //interactEvent.enabled = false;
        grabbable.enabled = false;
        physicsGrabbable.enabled = false;
        touchGrab.enabled = false;
        objectRespawn.enabled = false;

        Destroy(rg);
    }

    public void TurnOnComponents()
    {
        isAttached = false;

        //interactEvent.enabled = true;
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

    public void ChangeInteractableEventHandler(IInteractableView gameObj)
    {
        interactEvent.InjectAllInteractableUnityEventWrapper(gameObj);
        interactEvent.enabled = false;
        Invoke(nameof(EnableEventHandler), 0.1f);
    }

    public void EnableEventHandler()
    {
        interactEvent.enabled = true;
    }

}
