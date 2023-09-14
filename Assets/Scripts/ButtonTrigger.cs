using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent onPressed, onRelease;
    private Transform button;

    private Vector3 startPos;
    public Vector3 endPos;

    public bool isPressed;

    private float cooldownTime = 0.5f;

    private void Start()
    {
        isPressed = false;

        button = transform.GetChild(0);

        startPos = button.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPressed) return;
        Pressed();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isPressed) return;
        Released();
    }

    private void Pressed()
    {
        isPressed = true;
        button.localPosition = endPos;
        onPressed.Invoke();
        StartCoroutine(Cooldown());
        //StartCoroutine(LerpPos(startPos, endPos));
    }

    private void Released()
    {
        onRelease.Invoke();
    }

    private IEnumerator Cooldown()
    {
        //StartCoroutine(LerpPos(endPos, startPos));
        yield return new WaitForSeconds(cooldownTime);
        isPressed = false;
        button.localPosition = startPos;
    }

    private IEnumerator LerpPos(Vector3 start, Vector3 end)
    {
        float timer = 0;

        while (timer < cooldownTime)
        {
            button.localPosition = Vector3.Lerp(start, end, timer);
            timer += Time.deltaTime;
        }
        button.localPosition = end;

        yield return null;
    }
}
