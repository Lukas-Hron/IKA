using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> hourlyMusic = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;

    private TimerClock timer;
    [SerializeField] AudioClip timerMusic;

    int clipIndex = 0;

    private void Start()
    {
        clipIndex = Random.Range(0, hourlyMusic.Count);
        audioSource.clip = hourlyMusic[clipIndex];
        audioSource.Play();
        StartCoroutine(ChangeMusic());

        timer = FindObjectOfType<TimerClock>();

        timer.TimerStarted += OnTimerStarted;
        timer.TimerEnded += OnTimerEnded;
    }

    private IEnumerator ChangeMusic()
    {
        yield return new WaitForSeconds(60); // 20 min

        clipIndex++;
        audioSource.clip = hourlyMusic[clipIndex];
        audioSource.Play();
    }

    private void OnTimerStarted()
    {
        audioSource.clip = timerMusic;
        audioSource.Play();
    }

    private void OnTimerEnded()
    {
        audioSource.clip = hourlyMusic[clipIndex];
        audioSource.Play();
    }
}
