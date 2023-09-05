using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GluePump : MonoBehaviour
{
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    private AudioSource sound;

    private bool isPressed;
    private bool canBeInteracted;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        canBeInteracted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeInteracted)
            return;

        if (!isPressed && other.gameObject.CompareTag("Hand"))
        {
            canBeInteracted = false;
            isPressed = true;
            OnPress.Invoke();

            startRotation = transform.localRotation;
            targetRotation = Quaternion.Euler(90f, 0f, 0f);

            StartCoroutine(LerpRotation());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed && other.gameObject.CompareTag("Hand"))
        {
            isPressed = false;
            OnRelease.Invoke();

            startRotation = transform.localRotation;
            targetRotation = Quaternion.Euler(80f, 0f, 0f);

            StopAllCoroutines();
            StartCoroutine(LerpRotation());
        }
    }

    private IEnumerator LerpRotation()
    {
        float timer = 0;
        float duration = 0.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, timer / duration);
            yield return null;
        }

        canBeInteracted = true;
        transform.localRotation = targetRotation;
    }
}
