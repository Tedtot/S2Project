using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueID))]
public class TimeSystem : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI todText;

    public Action minuteChanged;
    public Action hourChanged;

    [SerializeField] private int minute;
    [SerializeField] private int hour;
    private bool meridiem; //am pm
    private string timeOfDay;

    private float minuteToReal = 0.5f;
    private float timer;

    private TimeSaveData timeSaveData;
    private string id;

    private void Awake() {
        SaveLoad.OnLoadGame += loadTime;
        timeSaveData = new TimeSaveData(new int[] {12, 0}, meridiem, timeOfDay);
    }

    private void loadTime(SaveData data) {
        //Check save data for specific data and loads it
        if (data.TimeSaveData.Time != null) {
            Debug.Log("load time");
            hour = data.TimeSaveData.Time[0];
            minute = data.TimeSaveData.Time[1];
            meridiem = data.TimeSaveData.Meridiem;
            timeOfDay = data.TimeSaveData.TimeOfDay;
        }
    }
    void Start() {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.TimeSaveData = new TimeSaveData(new int[] {12, 0}, meridiem, timeOfDay);

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

        SaveLoad.OnLoadGame += loadTime;

    }
    private void OnDisable() {
        minuteChanged -= UpdateTime;
        hourChanged -= UpdateTime;

        SaveLoad.OnLoadGame -= loadTime;
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

    public void updateSaveData() {
        SaveGameManager.data.TimeSaveData = new TimeSaveData(getTime(), meridiem, timeOfDay);
    }
}

[System.Serializable]
public struct TimeSaveData {
    public int[] Time;
    public bool Meridiem;
    public string TimeOfDay;

    public TimeSaveData(int[] time, bool meridiem, string timeOfDay) {
        Time = time;
        Meridiem = meridiem;
        TimeOfDay = timeOfDay;
    }
}
