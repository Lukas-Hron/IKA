using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlueDipper : MonoBehaviour
{
    private ParticleSpawner particleSpawn;
    [SerializeField] private GameObject clusterPrefab;
    [SerializeField]
    private GameObject glueDrippVFX;
    public int totalUses = 5;

    private void Start()
    {
        particleSpawn = GetComponent<ParticleSpawner>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpables"))
        {
            if (other.GetComponent<GlueAttacher>() == null)
            {
                GlueAttacher objRef = other.AddComponent<GlueAttacher>();

                objRef.clusterPrefab = this.clusterPrefab;
                objRef.parentParticles = particleSpawn;



                totalUses--;
            }
        }
    }


}
