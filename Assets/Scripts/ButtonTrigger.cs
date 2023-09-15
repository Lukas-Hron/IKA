using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent onPressed, onRelease;
    private Transform button;

    private Vector3 startPos;
    public Vector3 endPos;

    private bool isPressed;

    private void Start()
    {
        isPressed = false;


        button = transform.GetChild(0);


        startPos = button.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        Pressed();
    }

    private void OnTriggerExit(Collider other)
    {
        Released();
    }

    private void OnCollisionExit(Collision collision)
    {

    }


    void Pressed()
    {
        if (!isPressed)
        {
            isPressed = true;
            button.localPosition = endPos;
            onPressed.Invoke();
        }
    }


    void Released()
    {

        if (isPressed)
        {


            isPressed = false;
            button.localPosition = startPos;

            onRelease.Invoke();
        }
    }
}
