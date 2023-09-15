using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleSpawner : MonoBehaviour
{

    public GameObject[] VFXParticles;
    public GameObject[] systemParticles;

    private List<VisualEffect> VFXRefs = new List<VisualEffect>();
    public List<GameObject> vfxPosRef = new List<GameObject>();


    private void Start()
    {
        //vfxPosRef = Instantiate(VFXParticles, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < VFXParticles.Length; i++)
        {
            GameObject aaa = Instantiate(VFXParticles[i], Vector3.zero, Quaternion.identity);
            vfxPosRef.Add(aaa);
            VFXRefs.Add(vfxPosRef[i].GetComponent<VisualEffect>());

        }
    }


    public void PlayAllParticles(Vector3 position, int IndexVfx, int IndexSystem)
    {
        if (VFXParticles != null)
        {
            vfxPosRef[IndexVfx].transform.position = position;
            VFXRefs[IndexVfx].Play();
        }

        if (systemParticles != null)
        {
            Instantiate(systemParticles[IndexSystem], position, Quaternion.identity);
        }
    }

    public void PlayAllParticles(Vector3 position, int IndexVfx, int IndexSystem, bool playVFX)
    {
        if (playVFX)
        {
            if (VFXParticles != null)
            {
                vfxPosRef[IndexVfx].transform.position = position;
                Debug.Log("Org : " + vfxPosRef[IndexVfx].transform.position + " Target : " + position);
                VFXRefs[IndexVfx].Play();
            }
        }
        else
        {
            if (systemParticles != null)
            {
                Instantiate(systemParticles[IndexSystem], position, Quaternion.identity);
            }
        }
    }




}
