using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSause;

    public AudioClip[] audioClips;


    private void Awake()
    {
        audioSause = GetComponent<AudioSource>();
    }

    public void PlayAudio(int ClipIndex)
    {
        audioSause.clip = audioClips[ClipIndex];
        audioSause.Play();
    }

    public void PlayAudio(int ClipIndex, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClips[ClipIndex], position);
    }

}
