using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;
    [SerializeField] AudioSource alarmSource;

    public float timeRemaining = 600;

    private bool timerRunning;
    private bool alarmRunning;

    private void Start()
    {
        timerText.text = "00:00";
        timerRunning = true;

        alarmRunning = false;
        alarmSource.Stop();
        float alarmTimer = Random.Range(30, timeRemaining);
        Invoke(nameof(StartAlarm), alarmTimer);
    }

    private void StartAlarm()
    {
        alarmSource.Play();
        alarmRunning = true;
    }

    private void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        else
        {
            Debug.Log("timer ran out");
            timeRemaining = 0;
            timerRunning = false;
            CancelInvoke();
        }
    }

    public void TurnOffAlarm()
    {
        if (!alarmRunning) return;

        alarmSource.Stop();

        if (timerRunning)
        {
            float alarmTimer = Random.Range(0, timeRemaining / 2);
        alarmTimer = Mathf.Clamp(alarmTimer, 5, timeRemaining);
        Invoke(nameof(StartAlarm), alarmTimer);
        }
    }
}
