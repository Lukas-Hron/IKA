using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroanTube : MonoBehaviour
{
    [SerializeField] private float maxChange;
    private AudioSource audioSource;
    private Rigidbody rb;
    private Vector3 lastVelocity;

    #region Audio Clip Lists

    // List to hold audio clips for "Auuuu"
    [SerializeField] private List<AudioClip> Auuuu;

    // List to hold audio clips for "Uuuuua"
    [SerializeField] private List<AudioClip> Uuuua;

    // List to hold audio clips for "impact1"
    [SerializeField] private List<AudioClip> impact;

    // List to hold audio clips for "impact2"
    [SerializeField] private List<AudioClip> impact2;

    #endregion

    [SerializeField] private float minAngle = 0.25f;
    bool isDown = false;
    bool impactState = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckVelocityChange();
        CheckDotProduct();
        lastVelocity = rb.velocity;
    }

    private void CheckVelocityChange()
    {
        Vector3 velocity = rb.velocity;
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
        // Check if the list is empty or null
        if (audioList == null || audioList.Count == 0)
        {
            Debug.LogWarning("Audio list is empty or null.");
            return;
        }

        // Select a random index
        int randomIndex = Random.Range(0, audioList.Count);

        // Get the audio clip at the random index
        AudioClip selectedClip = audioList[randomIndex];

        // Play the selected audio clip
        PlaySfx(selectedClip);
    }
}
