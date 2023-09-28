using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBounce : MonoBehaviour
{
    [SerializeField] float bounce = 15;
    [SerializeField] bool useVelocityBounce;
    Rigidbody rb;
    Vector3 lastVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useVelocityBounce)
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal).normalized * bounce + Vector3.up;
        else
            rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal) * bounce;
    }
}