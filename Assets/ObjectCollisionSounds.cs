using UnityEngine;

public class ObjectCollisionSounds : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb == null)
        {
            return;
        }
        // Check if the object is stationary
        if (rb.velocity.magnitude == 0)
        {
            Debug.Log("Object is not moving!");
            return;
        }

        // Get the velocity magnitude normalized between 0 and 1 (assuming 1 m/s is the max speed for full volume)
        float volume = Mathf.Clamp01(rb.velocity.magnitude);

        // Get the contact point to play the sound
        Vector3 collisionPoint = collision.contacts[0].point;

        // Play the collision sound from the AudioManager
        //CollisionAudioManager.Instance.PlayCollisionSoundAt(collisionPoint, volume);
    }

    
}
