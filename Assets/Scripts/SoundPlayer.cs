using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSause;

    public AudioClip[] audioClips;

    private void OnEnable()
    {
        MenuScript musicMenu = GameObject.FindAnyObjectByType<MenuScript>();

        musicMenu.AddSFXVolume += AddVolume;
        musicMenu.RemoveSFXVolume += RemoveVolume;
    }

    private void OnDisable()
    {
        MenuScript musicMenu = GameObject.FindAnyObjectByType<MenuScript>();

        musicMenu.AddSFXVolume -= AddVolume;
        musicMenu.RemoveSFXVolume -= RemoveVolume;
    }

    private void Awake()
    {
        audioSause = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MenuScript musicMenu = GameObject.FindAnyObjectByType<MenuScript>();

        musicMenu.AddSFXVolume += AddVolume;
        musicMenu.RemoveSFXVolume += RemoveVolume;
    }

    private void AddVolume()
    {
        float targetVolume = Mathf.Clamp(audioSause.volume + 0.05f, 0.0f, 0.5f);
        float fadeDuration = 1.0f;
        StartCoroutine(FadeVolume(audioSause, targetVolume, fadeDuration));
    }

    private void RemoveVolume()
    {
        float targetVolume = Mathf.Clamp(audioSause.volume - 0.05f, 0.0f, 0.5f);
        float fadeDuration = 1.0f;
        StartCoroutine(FadeVolume(audioSause, targetVolume, fadeDuration));
    }

    IEnumerator FadeVolume(AudioSource audioSource, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / duration);
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        audioSource.volume = targetVolume;
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
