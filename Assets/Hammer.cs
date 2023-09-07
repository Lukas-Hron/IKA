using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the tag "PickUpable"
        if (other.CompareTag("PickUpable"))
        {
            // Check if the object has the "WeldableObject" component
            WeldableObject weldable = other.GetComponent<WeldableObject>();
            if (weldable != null)
            {
                // Check if the "isAttached" flag inside the WeldableObject component is false
                if (!weldable.isAttached)
                {
                    // Your logic here. For example:
                    ShootRay();
                    Debug.Log("Found an unattached WeldableObject!");
                }
            }
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);

            // Shoot another ray from the hit point in the direction of the normal
            Ray secondRay = new Ray(hit.point, hit.normal);
            RaycastHit secondHit;
                
            if (Physics.Raycast(secondRay, out secondHit, 100f))
            {
                Debug.DrawLine(secondRay.origin, secondHit.point, Color.blue, 2f);
            }
        }
    }
}

