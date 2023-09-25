using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooBox : MonoBehaviour
{
    private bool isPlaying;
    private bool startFrom1;
    [SerializeField] private float forceMultiplier;
    private float Acceleration;
    private float currentVelocity;
    private float audioClipLenght;
    private Vector3 lastVelocity;
    private float dotproduct;
    private float dotproductlastframe;

    Rigidbody rb;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        audioClipLenght = audio.clip.length;
        lastVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        dotproduct = Vector3.Dot(transform.up, Vector3.up);
        if (dotproduct != dotproductlastframe && isPlaying == false)
        {
            StartPlaying();
        }

        if (isPlaying)
        {
            UpdateAcceleration();
            currentVelocity = Mathf.MoveTowards(currentVelocity, Acceleration, 2f * Time.deltaTime);
            audio.pitch = currentVelocity;
            audio.volume = Mathf.Abs(currentVelocity);
            CheckAudioCompletion();
        }
        dotproductlastframe = dotproduct;
    }
    void UpdateAcceleration()
    {
        Acceleration = Mathf.Clamp(Vector3.Dot(transform.up, Vector3.up) * 1.1f, -1, 1);
        // Calculate the change in velocity and then divide by Time.deltaTime to get acceleration
        Vector3 accelerationVector = (rb.velocity - lastVelocity) / Time.deltaTime;

        // Take the dot product to get acceleration in the direction of the object's up vector
        Acceleration += Vector3.Dot(accelerationVector, transform.up) * Time.deltaTime;

        // For visual debugging, the ray length is mapped from acceleration magnitude
        Debug.DrawRay(transform.position, transform.up * Acceleration / 3, Color.red, Time.deltaTime, false);

        // Store the current velocity for the next frame
        lastVelocity = rb.velocity;
    }

    void StartPlaying()
    {
        audio.Play();
        isPlaying = true;
        if (startFrom1)
            audio.time = audioClipLenght - 0.1f;
        else
            audio.time = 0.1f;
    }
    void CheckAudioCompletion()
    {
        // Playing forwards
        if (audio.pitch > 0 && audio.time >= audioClipLenght - 0.05f)
        {
            audio.Pause();
            audio.time = audioClipLenght - 0.1f;
            isPlaying = false;
        }
        // Playing in reverse
        else if (audio.pitch < 0 && audio.time <= 0.05f)
        {
            audio.Pause();
            audio.time = 0.1f;
            isPlaying = false;
        }
    }

}

