using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;
    [SerializeField] AudioSource alarmSource;

    public float timeRemaining = 60;
    private float alarmTime = 0;

    private bool timerRunning;

    private void Start()
    {
        timerText.text = "00:00";
        timerRunning = true;

        alarmSource.Stop();
        Invoke(nameof(GetRandomTimer), 5);
    }

    private void GetRandomTimer()
    {
        //dont run if theres less then 10 seconds left or its not running
        if (timeRemaining < 10 || !timerRunning) return;

        alarmTime = Random.Range(10, timeRemaining - 10);
        alarmTime = Mathf.Clamp(alarmTime, 10, timeRemaining - 10);

    }

    private void Update()
    {
        if (!timerRunning) return;

        if(timeRemaining < alarmTime)
        {
            alarmSource.Play();
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = "0" + minutes + ":" + seconds;

        }
        else
        {
            Debug.Log("timer ran out");
            timeRemaining = 0;
            timerRunning = false;
        }
    }

    public void TurnOffAlarm()
    {

    }
}
