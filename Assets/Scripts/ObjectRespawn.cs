using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    private Transform Origin;
    private Rigidbody rg;

    private void OnEnable()
    {
        RespawnAll.Respawn += Respawn;
    }
    private void OnDisable()
    {
        RespawnAll.Respawn -= Respawn;
    }
    private void Start()
    {
        Origin = transform;

        rg = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        gameObject.transform.position = Origin.position;
        gameObject.transform.rotation = Origin.rotation;

        if (rg != null)
            rg.velocity = Vector3.zero;
    }
}
