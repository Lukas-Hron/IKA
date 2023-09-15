using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSause;

    public AudioClip[] audioClips;


    private void Start()
    {
        audioSause = GetComponent<AudioSource>();
    }

    public void PlayAudio(int ClipIndex)
    {
        audioSause.clip = audioClips[ClipIndex];
        audioSause.Play();
    }



}
