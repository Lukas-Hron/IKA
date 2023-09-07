using Meta.WitAi.CallbackHandlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPhysics : MonoBehaviour
{

    public UnityEvent onPressed, onRelease;

    [SerializeField] private float threshhold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;

    private bool isPressed;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;


        if (Mathf.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }


    private void Start()
    {
        startPos = transform.position;
        joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {


        if (!isPressed && GetValue() + threshhold >= 1)
            Pressed();
        else if (isPressed && GetValue() - threshhold <= 0)
            Released();


    }


    private void Pressed()
    {
        isPressed = true;
        onPressed.Invoke();
        Debug.Log("Pressed");
    }
    private void Released()
    {
        isPressed = false;
        onRelease.Invoke();
        Debug.Log("Release");
    }



}
