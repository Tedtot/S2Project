using UnityEngine;
using System;
using TMPro;

public class TimeSystem : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI todText;

    public Action minuteChanged;
    public Action hourChanged;

    private int minute;
    private int hour;
    private bool meridiem; //am pm
    private string timeOfDay;

    private float minuteToReal = 1f;
    private float timer;

    void Start() {
        timeText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        todText = GameObject.Find("TimeOfDay").GetComponent<TextMeshProUGUI>();

        minute = 0;
        hour = 12;
        meridiem = false; //am
        timeOfDay = "Night";

        timer = minuteToReal;
    }

    void Update() {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            minute++;
            minuteChanged?.Invoke();

            if (minute >= 60) {
                hour++;
                minute = 0;

                if (hour == 12) meridiem = !meridiem;
                else if (hour > 12) hour = 1;
                
                hourChanged?.Invoke();
            }
            timer = minuteToReal;
        }
    }

    private void OnEnable() {
        minuteChanged += UpdateTime;
        hourChanged += UpdateTime;
    }
    private void OnDisable() {
        minuteChanged -= UpdateTime;
        hourChanged -= UpdateTime;
    }
    private void UpdateTime() {
        string t = " AM";
        if (meridiem) t = " PM";

        switch(hour, meridiem) {
            case (10, true):
                timeOfDay = "Night";
                break;
            case (5, false):
                timeOfDay = "Morning";
                break;
            case (12, true):
                timeOfDay = "Noon";
                break;
            case (2, true):
                timeOfDay = "Afternoon";
                break;
            case (7, true):
                timeOfDay = "Evening";
                break;
        }
        timeText.text = $"{hour:00}:{minute:00}" + t;
        todText.text = timeOfDay;
    }
    public int[] getTime() {
        return new int[] { hour, minute };
    }
}
