using UnityEngine;
using System.Collections.Generic;

public class CollisionAudioManager : MonoBehaviour
{
    // Singleton instance
    public static CollisionAudioManager Instance { get; private set; }

    public List<AudioClip> collisionSounds = new List<AudioClip>();

    // Reference to the AudioSource attached to this GameObject
    private AudioSource sourceReference;

    private void Awake()
    {
        // Ensure there's only one instance of AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sourceReference = GetComponent<AudioSource>();
        if (sourceReference == null)
        {
            Debug.LogError("No AudioSource component found on AudioManager!");
        }
    }

    public void PlayCollisionSoundAt(Vector3 pos, float velocityMagnitude)
    {
        // Randomize sound
        AudioClip clip = collisionSounds[Random.Range(0, collisionSounds.Count)];

        PlayCustomOneShot(clip, pos, velocityMagnitude);
    }

    private void PlayCustomOneShot(AudioClip clip, Vector3 position, float volumeMultiplier)
    {
        GameObject soundObject = new GameObject("TempAudio");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        // Set audioSource settings based on the reference AudioSource
        audioSource.spatialBlend = sourceReference.spatialBlend;
        audioSource.minDistance = sourceReference.minDistance;
        audioSource.maxDistance = sourceReference.maxDistance;
        audioSource.rolloffMode = sourceReference.rolloffMode;

        // Modify the volume based on velocityMagnitude (you can adjust this formula as needed)
        audioSource.volume = Mathf.Clamp01(sourceReference.volume * volumeMultiplier);

        audioSource.clip = clip;
        audioSource.Play();

        // Destroy the temporary audio source after the clip finishes
        Destroy(soundObject, clip.length);
    }
}

