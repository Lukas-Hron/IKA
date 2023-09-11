using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawnHitbox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "PickUpables"
        if (collision.gameObject.CompareTag("PickUpables"))
        {
            // Try to get the ObjectRespawn script attached to the object
            ObjectRespawn respawnScript = collision.gameObject.GetComponent<ObjectRespawn>();

            // If the script exists, call its reset method
            if (respawnScript != null)
            {
                respawnScript.Respawn();
            }
        }
    }
}
