using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papperskorg : MonoBehaviour
{
    //public GameObject Hammer;

    public bool realTrashcan = false;

    private ParticleSpawner partSpawn;
    private SoundPlayer soundPlay;

    private void Start()
    {
        if (!realTrashcan) return;
        partSpawn = GetComponent<ParticleSpawner>();
        soundPlay = GetComponent<SoundPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUpables"))
        {
            if (other.transform.root.gameObject.GetComponent<Cluster>())
                Destroy(other.transform.root.gameObject);

            else
                Destroy(other.gameObject);

            if (realTrashcan)
            {
                partSpawn.PlayOneParticles(transform.position, 0, true);
                soundPlay.PlayAudio(0);
            }
        }
    }


}
