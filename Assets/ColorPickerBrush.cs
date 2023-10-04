using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerBrush : MonoBehaviour
{
    public Transform raycastOrigin; // Assign the transform from where you want to send the ray.
    public Material brushMaterial;
    public Color currentColor;      // This is the color picked up from the texture.

    public bool isWatered;

    private bool isGrabbed;
    public void SetIsGrabbedBool(bool value) => isGrabbed = value;

    private void Start()
    {
        brushMaterial.color = Color.white;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGrabbed) return;

        if (other.CompareTag("Water"))
        {
            isWatered = true;
            brushMaterial.color = Color.white;
        }


        //Debug.Log("Found " + other.name);
        if (other.CompareTag("ColorPickerTexture")) // Make sure to tag your rainbow gradient texture object with "ColorPickerTexture".
        {
            isWatered = false;

            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * 10f, Color.red, 1f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == other)
                {
                    Texture2D texture = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;

                    if (texture != null)
                    {
                        Vector2 pixelUV = hit.textureCoord;
                        Debug.Log(hit.textureCoord);
                        pixelUV.x *= texture.width;
                        pixelUV.y *= texture.height;

                        currentColor = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                        brushMaterial.color = currentColor;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbed) return;

        Colorable colorable = other.GetComponent<Colorable>();

        if (colorable != null && isWatered)
            colorable.Renderer.material.color = colorable.originColor;

        else if (colorable != null)
            colorable.Renderer.material.color = currentColor;
    }
}
