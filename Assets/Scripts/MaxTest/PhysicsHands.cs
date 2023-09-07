using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHands : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public Transform handToFollow;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = (handToFollow.position - transform.position)/Time.fixedDeltaTime;

        Quaternion rotationDifference = handToFollow.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegrees = angleInDegree * rotationAxis;

        rb.angularVelocity = rotationDifferenceInDegrees  * Mathf.Deg2Rad / Time.fixedDeltaTime;   

    }
}
