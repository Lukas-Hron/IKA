using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckForWeld : MonoBehaviour
{
    public bool isHeld;

    private GameObject objectToAttachTo;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public LayerMask layermask = 1 << 6;//WeldableObjects Layer

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WeldableObject>() != null)
        {
            //Show highlight line or smth that shows where this will attatch to

            //send raycast and get the normal of that 
            RaycastHit hit;
            Vector3 rayDirection = (other.transform.position - transform.position).normalized;
            //Vector3 rayDirection = (FindClosestPoint(other.gameObject) - FindClosestPoint(this.gameObject)).normalized;

            float rayDistance = (other.transform.position - transform.position).magnitude;

            if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance, layermask))
            {
                Debug.Log("hej");
                Debug.DrawRay(transform.position, rayDirection, Color.blue, 10);
            }

            CalculateAttach(other.gameObject);
            Debug.Break();

            objectToAttachTo = other.gameObject;
            //StartCoroutine(LerpAttachPosition());
        }
    }

    private void CalculateAttach(GameObject otherPiece)
    {
        BoxCollider thisCollider = gameObject.GetComponent<BoxCollider>();
        Vector3 closestPointThis = thisCollider.ClosestPoint(otherPiece.transform.position);

        BoxCollider otherCollider = gameObject.GetComponent<BoxCollider>();
        Vector3 closestPointOther = thisCollider.ClosestPoint(transform.position);


        targetPosition = closestPointThis - closestPointOther;
        targetRotation = Quaternion.FromToRotation(transform.up, otherPiece.transform.up);

    }

    private Vector3 FindClosestPointToThis(Vector3 position)
    {
        BoxCollider thisCollider = GetComponent<BoxCollider>();

        Vector3 closestPoint = thisCollider.ClosestPoint(position);

        closestPoint -= thisCollider.center;
        Debug.Log("this " + gameObject.name + " " + closestPoint);
        Gizmos.DrawWireSphere(closestPoint, 0.1f);
        return closestPoint;

    }
    private Vector3 FindClosestPointTo(GameObject objectToCheck)
    {
        BoxCollider otherCollider = objectToCheck.GetComponent<BoxCollider>();

        Vector3 closestPoint = otherCollider.ClosestPoint(objectToCheck.transform.position);

        closestPoint -= otherCollider.center;

        closestPoint = objectToCheck.transform.TransformPoint(closestPoint);
        Debug.Log("other " + objectToCheck.name + " " + closestPoint);
        return closestPoint;

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, 0.1f);

    }
    private IEnumerator LerpAttachPosition()
    {
        float duration = 1f;
        float timer = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, timer / duration);
            transform.rotation = Quaternion.Slerp(startRot, targetRotation, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos + targetPosition;
        transform.rotation = startRot * targetRotation;

        objectToAttachTo.GetComponent<WeldableObject>().AttachToObject(this.gameObject);
    }

}
