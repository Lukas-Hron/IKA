using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phand : MonoBehaviour
{
    public Transform trackedTransform = null;
    public Rigidbody body = null;

    public float positionStrength = 20;
    public float rotationStrength = 30;


    void FixedUpdate()
    {
        var vel = (trackedTransform.position - body.position).normalized * positionStrength * Vector3.Distance(trackedTransform.position, body.position);
        body.velocity = vel;


        float kp = (6f * rotationStrength) * (6f * rotationStrength) * 0.25f;
        float kd = 4.5f * rotationStrength;
        Vector3 x;
        float xMag;
        Quaternion q = trackedTransform.rotation * Quaternion.Inverse(transform.rotation);
        q.ToAngleAxis(out xMag, out x);
        x.Normalize();
        x *= Mathf.Deg2Rad;
        Vector3 pidv = kp * x * xMag - kd * body.angularVelocity;
        Quaternion rotInertia2World = body.inertiaTensorRotation * transform.rotation;
        pidv = Quaternion.Inverse(rotInertia2World) * pidv;
        pidv.Scale(body.inertiaTensor);
        pidv = rotInertia2World * pidv;
        body.AddTorque(pidv);
    }
}


public class PhysicsHand : MonoBehaviour
{
    public Transform trackedTransform = null;
    public Rigidbody body = null;

    public float positionStrength = 20;
    public float positionThreshold = 0.005f;
    public float maxDistance = 1f;
    public float rotationStrength = 30;
    public float rotationThreshold = 10f;

    void FixedUpdate()
    {
        var distance = Vector3.Distance(trackedTransform.position, body.position);
        if (distance > maxDistance || distance < positionThreshold)
        {
            body.MovePosition(trackedTransform.position);
        }
        else
        {
            var vel = (trackedTransform.position - body.position).normalized * positionStrength * distance;
            body.velocity = vel;
        }

        float angleDistance = Quaternion.Angle(body.rotation, trackedTransform.rotation);
        if (angleDistance < rotationThreshold)
        {
            body.MoveRotation(trackedTransform.rotation);
        }
        else
        {
            float kp = (6f * rotationStrength) * (6f * rotationStrength) * 0.25f;
            float kd = 4.5f * rotationStrength;
            Vector3 x;
            float xMag;
            Quaternion q = trackedTransform.rotation * Quaternion.Inverse(transform.rotation);
            q.ToAngleAxis(out xMag, out x);
            x.Normalize();
            x *= Mathf.Deg2Rad;
            Vector3 pidv = kp * x * xMag - kd * body.angularVelocity;
            Quaternion rotInertia2World = body.inertiaTensorRotation * transform.rotation;
            pidv = Quaternion.Inverse(rotInertia2World) * pidv;
            pidv.Scale(body.inertiaTensor);
            pidv = rotInertia2World * pidv;
            body.AddTorque(pidv);
        }
    }
}
