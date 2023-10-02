using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> hourlyMusic = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip timerMusic;
    private TimerClock timer;

    int clipIndex = 0;

    private void Start()
    {
        clipIndex = DateTime.Now.Hour - 1;

        PlayMusicClip(hourlyMusic[clipIndex]);

        timer = FindObjectOfType<TimerClock>();

        if (timer != null)
        {
            timer.TimerStarted += OnTimerStarted;
            timer.TimerEnded += OnTimerEnded;
        }
    }

    private void PlayMusicClip(AudioClip clipToPlay)
    {
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    private float GetTimeTillNextHour()
    {
        TimeSpan timeUntilChange = TimeSpan.FromMinutes(60 - DateTime.Now.Minute);

        return (float)timeUntilChange.TotalSeconds;
    }

    private IEnumerator ChangeMusic()
    {
        float timeUntilChange = GetTimeTillNextHour();
        yield return new WaitForSeconds(timeUntilChange);

        clipIndex = DateTime.Now.Hour - 1;
        PlayMusicClip(hourlyMusic[clipIndex]);

        StartCoroutine(ChangeMusic());
    }



    private void OnTimerStarted()
    {
        PlayMusicClip(timerMusic);

        StopAllCoroutines();
    }

    private void OnTimerEnded()
    {
        PlayMusicClip(hourlyMusic[clipIndex]);

        StartCoroutine(ChangeMusic());
    }
}
