using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroanTube : MonoBehaviour
{
    [SerializeField] private float maxChange;
    private AudioSource audioSource;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    #region Audio Clip Lists

    [SerializeField] private List<AudioClip> Auuuu;
    [SerializeField] private List<AudioClip> Uuuua;
    [SerializeField] private List<AudioClip> impact;
    [SerializeField] private List<AudioClip> impact2;

    #endregion

    [SerializeField] private float minAngle = 0.25f;
    bool isDown = false;
    bool impactState = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position; // Initialize lastPosition
    }

    private void FixedUpdate()
    {
        // Calculate velocity manually if Rigidbody is kinematic
        if (rb.isKinematic)
        {
            lastVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
            lastPosition = transform.position;
        }
        else
        {
            lastVelocity = rb.velocity;
        }

        CheckVelocityChange();
        CheckDotProduct();
    }

    private void CheckVelocityChange()
    {
        // Use lastVelocity, which is updated based on whether the Rigidbody is kinematic or not
        Vector3 velocity = lastVelocity;
        float dif = Mathf.Abs((velocity - lastVelocity).magnitude) / Time.fixedDeltaTime;

        if (dif > maxChange)
        {
            if (impactState) SelectRandomSfx(impact);
            else SelectRandomSfx(impact2);

            impactState = !impactState;
        }
    }

    private void CheckDotProduct()
    {
        float dot = Vector3.Dot(transform.position.normalized, transform.up);

        if (isDown)
        {
            if (dot >= minAngle)
            {
                SelectRandomSfx(Auuuu);
                isDown = false;
            }
        }
        else
        {
            if (dot <= -minAngle)
            {
                SelectRandomSfx(Uuuua);
                isDown = true;
            }
        }
    }

    private void PlaySfx(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void SelectRandomSfx(List<AudioClip> audioList)
    {
        if (audioList == null || audioList.Count == 0)
        {
            Debug.LogWarning("Audio list is empty or null.");
            return;
        }

        int randomIndex = Random.Range(0, audioList.Count);
        AudioClip selectedClip = audioList[randomIndex];
        PlaySfx(selectedClip);
    }
}
