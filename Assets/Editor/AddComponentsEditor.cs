using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Oculus.Interaction.TwoGrabFreeTransformer;

public class AddComponentsToObject : Editor
{
    [MenuItem("GameObject/Add Components")]
    static void AddComponentsToGameObject()
    {
        //Get selected object in scene
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            selectedObject.tag = "PickUpables";

            if (!selectedObject.GetComponent<Rigidbody>())
            {
                var rb = selectedObject.AddComponent<Rigidbody>();
                rb.mass = 0.5f;
            }

            if (!selectedObject.GetComponent<WeldableObject>())
            {
                selectedObject.AddComponent<WeldableObject>();
            }

            if (!selectedObject.GetComponent<OneGrabFreeTransformer>())
            {
                selectedObject.AddComponent<OneGrabFreeTransformer>();
            }

            if (!selectedObject.GetComponent<TwoGrabFreeTransformer>())
            {
                TwoGrabFreeTransformer twoGrab = selectedObject.AddComponent<TwoGrabFreeTransformer>();

                TwoGrabFreeConstraints twoGrabConstraints = new TwoGrabFreeConstraints();

                twoGrabConstraints.MinScale.Constrain = true;
                twoGrabConstraints.MinScale.Value = 1f;

                twoGrabConstraints.MaxScale.Constrain = true;
                twoGrabConstraints.MaxScale.Value = 10f;

                twoGrab.Constraints = twoGrabConstraints;
            }

            if (!selectedObject.GetComponent<Grabbable>())
            {
                var grabbable = selectedObject.AddComponent<Grabbable>();

                grabbable.InjectOptionalOneGrabTransformer(selectedObject.GetComponent<OneGrabFreeTransformer>());
                grabbable.InjectOptionalTwoGrabTransformer(selectedObject.GetComponent<TwoGrabFreeTransformer>());
            }

            if (!selectedObject.GetComponent<PhysicsGrabbable>())
            {
                selectedObject.AddComponent<PhysicsGrabbable>();
            }

            if (!selectedObject.GetComponent<TouchHandGrabInteractable>())
            {
                var touchGrab = selectedObject.AddComponent<TouchHandGrabInteractable>();
                touchGrab.SetSelfComponents = true;
            }

            if (!selectedObject.GetComponent<ObjectRespawn>())
            {
                selectedObject.AddComponent<ObjectRespawn>();
            }

            if (!selectedObject.GetComponent<Colorable>())
            {
                selectedObject.AddComponent<Colorable>();
            }

            if (!selectedObject.GetComponent<InteractableUnityEventWrapper>())
            {
                
                var eventWrapper = selectedObject.AddComponent<InteractableUnityEventWrapper>();
                eventWrapper.InjectInteractableView(selectedObject.GetComponent<TouchHandGrabInteractable>());
            }
        }
    }
}
