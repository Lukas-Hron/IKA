using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingWand : MonoBehaviour
{
    private bool isGrabbed;
    public void SetIsGrabbedBool(bool value) { isGrabbed = value; }

    private ParticleSpawner particleScript;
    private SoundPlayer soundScript;

    private void Start()
    {
        particleScript = GetComponent<ParticleSpawner>();
        soundScript = GetComponent<SoundPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        soundScript.PlayAudio(0);

        if (!isGrabbed) return;

        if (other.GetComponent<WeldableObject>() != null)// get object and remove it from the cluster if attached
        {
            if (other.GetComponent<WeldableObject>().isAttached)
            {
                other.GetComponentInParent<Cluster>().RemovePartFromList(other.gameObject);

                soundScript.PlayAudio(1);
                particleScript.PlayOneParticles(other.ClosestPoint(transform.position), 0, true);
            }
        }
    }
}
