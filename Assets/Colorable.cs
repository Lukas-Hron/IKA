using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorable : MonoBehaviour
{
    public Renderer Renderer;
    // Start is called before the first frame update
    void Start()
    {
        if (Renderer == null)
        {
            Renderer = GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
