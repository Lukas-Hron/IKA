using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;

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
        originPos = transform.position;
        originRot = transform.rotation;

        rg = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        gameObject.transform.position = originPos;
        gameObject.transform.rotation = originRot;

        if (rg != null)
            rg.velocity = Vector3.zero;
    }
}
