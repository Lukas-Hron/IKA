using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;
    [SerializeField] AudioSource alarmSource;

    public float timer = 0;
    private float maxAlarmTime = 400;

    private bool timerRunning;
    private bool alarmRunning;

    private void Start()
    {
        timerText.text = "00:00";
        timerRunning = true;

        alarmRunning = false;
        alarmSource.Stop();
        float alarmTimer = Random.Range(30, maxAlarmTime);
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

        if (timer > 0)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
    }

    public void TurnOffAlarm()
    {
        if (!alarmRunning) return;

        alarmSource.Stop();

        if (timerRunning)
        {
            float alarmTimer = Random.Range(30, maxAlarmTime);
            Invoke(nameof(StartAlarm), alarmTimer);
        }
    }
}
