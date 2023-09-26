using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleSpawner : MonoBehaviour
{
    


    public GameObject[] VFXParticles;
    public GameObject[] systemParticles;

    private List<VisualEffect> VFXRefs = new List<VisualEffect>();
    private List<GameObject> vfxPosRef = new List<GameObject>();


    private void Start()
    {
        //vfxPosRef = Instantiate(VFXParticles, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < VFXParticles.Length; i++)
        {
            GameObject temp = Instantiate(VFXParticles[i], Vector3.zero, Quaternion.identity);
            vfxPosRef.Add(temp);
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

    public void PlayAllParticles(Vector3 position, int Index, bool VFXOnly)
    {
        if (VFXOnly)
        {
            if (VFXParticles != null)
            {
                vfxPosRef[Index].transform.position = position;
                Debug.Log("Org : " + vfxPosRef[Index].transform.position + " Target : " + position);
                VFXRefs[Index].Play();
            }
        }
        else
        {
            if (systemParticles != null)
            {
                Instantiate(systemParticles[Index], position, Quaternion.identity);
            }
        }
    }




}
