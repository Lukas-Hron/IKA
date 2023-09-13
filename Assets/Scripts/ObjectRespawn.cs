using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    private Vector3 OriginPos;
    private Quaternion OriginRot;
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
        OriginPos = transform.position;
        OriginRot = transform.rotation;

        rg = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        gameObject.transform.position = OriginPos;
        gameObject.transform.rotation = OriginRot;

        if (rg != null)
            rg.velocity = Vector3.zero;
    }
}
