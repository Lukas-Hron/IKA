using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    public List <WeldableObject> WeldObjects;
    public HandVisual hand;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //WeldObjects[0].AttachToObject(WeldObjects[1].gameObject, false);
        }
        
    }
    
}
