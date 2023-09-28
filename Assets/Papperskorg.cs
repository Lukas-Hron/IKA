using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papperskorg : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PickUpables"))
            Destroy(other.gameObject);
    }
}
