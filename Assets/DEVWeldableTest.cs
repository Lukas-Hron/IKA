using UnityEngine;

public class DEVWeldableTest : MonoBehaviour
{
    public GameObject objectToAttach;
    public GameObject targetObject;
    public GameObject objectToRemoveFrom;

    [Header("Buttons (Enable to use)")]
    public bool attachButton = false;
    public bool removeButton = false;

    void Update()
    {
        // Check if attachButton is enabled
        if (attachButton)
        {
            if (objectToAttach != null && targetObject != null)
            {
                WeldableObject weldable = objectToAttach.GetComponent<WeldableObject>();
                if (weldable != null)
                {
                    weldable.AttachToObject(targetObject);
                }
                else
                {
                    Debug.LogError("ObjectToAttach does not have a WeldableObject component!");
                }
            }
            else
            {
                Debug.LogError("Either objectToAttach or targetObject is not set!");
            }
            attachButton = false; // Reset the button
        }

        // Check if removeButton is enabled
        if (removeButton)
        {
            if (objectToRemoveFrom != null)
            {
                WeldableObject weldable = objectToRemoveFrom.GetComponent<WeldableObject>();
                if (weldable != null)
                {
                    weldable.RemoveFromObject();
                }
                else
                {
                    Debug.LogError("ObjectToRemoveFrom does not have a WeldableObject component!");
                }
            }
            else
            {
                Debug.LogError("ObjectToRemoveFrom is not set!");
            }
            removeButton = false; // Reset the button
        }
    }
}