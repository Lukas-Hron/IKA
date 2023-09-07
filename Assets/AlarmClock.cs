using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] TextMeshPro timerText;

    public float timeRemaining = 60;
    private bool timerRunning;

    private void Start()
    {
        timerText.text = "00:00";
        timerRunning = true;
    }

    private void Update()
    {
        if (!timerRunning) return;


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

}
