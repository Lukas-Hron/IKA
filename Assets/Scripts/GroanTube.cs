using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroanTube : MonoBehaviour
{
    [SerializeField] private float maxChange;
    private AudioSource audioSource;
    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private Vector3 currentVelocity;

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
        lastPosition = transform.position; // Initialize lastPosition
    }

    private void FixedUpdate()
    {
        currentVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

        CheckVelocityChange();
        CheckDotProduct();

        lastVelocity = currentVelocity;
    }

    private void CheckVelocityChange()
    {
        Vector3 velocity = currentVelocity;
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
