using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlueDipper : MonoBehaviour
{
    private ParticleSpawner particleSpawn;
    private SoundPlayer soundPlay;
    [SerializeField] private GameObject clusterPrefab;
    [SerializeField]
    public int totalUses = 5;



    private void Start()
    {
        particleSpawn = GetComponent<ParticleSpawner>();
        soundPlay = GetComponent<SoundPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpables"))
        {
            soundPlay.PlayAudio(0);
            if (other.GetComponent<GlueAttacher>() == null)
            {
                GlueAttacher objRef = other.transform.parent.AddComponent<GlueAttacher>();
                objRef.clusterPrefab = this.clusterPrefab;
                objRef.parentParticles = particleSpawn;
                objRef.parentSound = soundPlay;

                totalUses--;
            }
        }
    }


}
