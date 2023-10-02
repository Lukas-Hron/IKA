using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerBrush : MonoBehaviour
{
    public Transform raycastOrigin; // Assign the transform from where you want to send the ray.
    public Material brushMaterial;
    public Color currentColor;      // This is the color picked up from the texture.

    private void Start()
    {
        brushMaterial.color = Color.white;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Found " + other.name);
        if (other.CompareTag("ColorPickerTexture")) // Make sure to tag your rainbow gradient texture object with "ColorPickerTexture".
        {
            Debug.Log("Gameobject is Tagged correctly");
            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * 10f, Color.red, 1f);


            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == other)
                {
                    Debug.Log("Hit " + other.name);
                    Texture2D texture = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;

                    if (texture != null)
                    {
                        Debug.Log("Texture isn't null");
                        Vector2 pixelUV = hit.textureCoord;
                        Debug.Log(hit.textureCoord);
                        pixelUV.x *= texture.width;
                        pixelUV.y *= texture.height;

                        currentColor = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                        Debug.Log("Color is " + currentColor);
                        brushMaterial.color = currentColor;
                    }
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Colorable colorable = other.GetComponent<Colorable>();
            if (colorable != null)
            {
                colorable.Renderer.material.color = currentColor;
            }
    }
}
