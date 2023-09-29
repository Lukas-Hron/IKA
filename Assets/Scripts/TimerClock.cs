using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerClock : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;
    [SerializeField] AudioSource timerSource;

    [SerializeField] AudioClip timerStart;
    [SerializeField] AudioClip timerEnd;

    public event Action TimerStarted;
    public event Action TimerEnded;

    private float timer = 0;
    private bool timerRunning;

    private void Start()
    {
        timerText.text = "00:00";
        timerSource.Stop();
    }

    public void StartTimer()
    {
        timerText.text = "00:00";

        TimerStarted?.Invoke();
        timerRunning = true;

        timerSource.clip = timerStart;
        timerSource.Play();

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (timerRunning)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        yield return null;
    }

    public void StopTimer()
    {
        TimerEnded?.Invoke();
        timerRunning = false;

        timerSource.clip = timerEnd;
        timerSource.Play();
    }
}
