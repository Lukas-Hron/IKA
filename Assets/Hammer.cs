using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the tag "PickUpable"
        if (other.CompareTag("PickUpables"))
        {
            // Check if the object has the "WeldableObject" component
            WeldableObject weldable = other.GetComponent<WeldableObject>();
            if (weldable != null)
            {
                // Check if the "isAttached" flag inside the WeldableObject component is false
                if (!weldable.isAttached)
                {
                    // Your logic here. For example:
                    ShootRay(other.gameObject);
                    Debug.Log("Found an unattached WeldableObject!");
                }
            }
        }
    }

    void ShootRay(GameObject other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
            Debug.Log("Found original weldable");
            // Shoot another ray from the hit point in the direction of the normal
            Ray secondRay = new Ray(hit.point, hit.normal * -1f);
            RaycastHit secondHit;

            if (Physics.Raycast(secondRay, out secondHit, 100f))
            {
                Debug.DrawLine(secondRay.origin, secondHit.point, Color.blue, 2f);
                Debug.Log("WE BE WELDIN'");

                hit.collider.gameObject.GetComponent<WeldableObject>().AttachToObject(secondHit.collider.gameObject);
            }
        }
    }
}

