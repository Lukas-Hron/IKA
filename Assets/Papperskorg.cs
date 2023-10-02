using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papperskorg : MonoBehaviour
{
    public GameObject Hammer;
    private void OnCollisionEnter(Collision other)
    {
        ObjectRespawn op = other.gameObject.GetComponent<ObjectRespawn>();

        if(other.gameObject.GetComponent<ColorPickerBrush>() != null)
             op?.Respawn();
        else if(other.gameObject.GetComponent<RemovingWand>())
            op?.Respawn();
        else if(other.gameObject == Hammer)
            other.gameObject.GetComponent<ObjectRespawn>().Respawn();


        if (other.gameObject.CompareTag("PickUpables"))
            Destroy(other.gameObject);
    }
}
