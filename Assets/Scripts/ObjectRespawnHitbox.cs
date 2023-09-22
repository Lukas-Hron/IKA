using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawnHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "PickUpables"
        if (other.gameObject.CompareTag("PickUpables"))
        {
            // Try to get the ObjectRespawn script attached to the object
            ObjectRespawn respawnScript = other.gameObject.GetComponent<ObjectRespawn>();

            // If the script exists, call its reset method
            if (respawnScript != null)
            {
                respawnScript.Respawn();
            }
        }
    }
}
