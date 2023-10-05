using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorable : MonoBehaviour
{
    public Renderer Renderer;
    public Color originColor;

    void Start()
    {
        if (Renderer == null)
        {
            Renderer = GetComponent<Renderer>();
        }

        Material mat = Renderer.material;

        if (mat.HasProperty("_BaseColor"))
        {
            originColor = mat.GetColor("_BaseColor");
        }
    }
}
