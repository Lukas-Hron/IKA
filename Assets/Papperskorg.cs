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
        if(!realTrashcan) return;
        partSpawn = GetComponent<ParticleSpawner>();
        soundPlay = GetComponent<SoundPlayer>();    
    }

    //private void TriggerEnter(Collision other)
    //{
    //    //ObjectRespawn op = other.gameObject.GetComponent<ObjectRespawn>();

    //    //if(other.gameObject.GetComponent<ColorPickerBrush>() != null)
    //    //     op?.Respawn();
    //    //else if(other.gameObject.GetComponent<RemovingWand>())
    //    //    op?.Respawn();
    //    //else if(other.gameObject == Hammer)
    //    //    other.gameObject.GetComponent<ObjectRespawn>().Respawn();

    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUpables"))
        {
            Destroy(other.transform.root.gameObject);
            if (realTrashcan)
            {
                partSpawn.PlayOneParticles(transform.position, 0, true);
                soundPlay.PlayAudio(0);
            }
        }
    }


}
