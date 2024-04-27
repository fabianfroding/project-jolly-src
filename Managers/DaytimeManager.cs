using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private SOIntVariable currentHour;
    [SerializeField] private SOIntVariable currentMinute;
    [SerializeField] private SOGameEvent onHourChanged;
    [SerializeField] private SOGameEvent onMinuteChanged;

    [Range(5, 9)]
    [SerializeField] private float dawnStartTime = 0f;
    [Range(9, 12)]
    [SerializeField] private float dawnEndTime = 0f;
    [Range(16, 18)]
    [SerializeField] private float duskStartTime = 0f;
    [Range(18, 20)]
    [SerializeField] private float duskEndTime = 0f;

    private float gameHourInterval;
    private float gameMinuteInterval;

    public bool stopDaytime = false;

    private static DaytimeManager instance;
    public static DaytimeManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<DaytimeManager>();
            return instance;
        }
    }

    private void Start()
    {
        gameHourInterval = realMinutesPerDay / 24.0f * 60.0f;
        gameMinuteInterval = gameHourInterval / 60f;

        if (SaveManager.DoesPlayerSaveDataExist())
        {
            PlayerSaveData playerSaveData = SaveManager.LoadPlayerSaveData();
            SetCurrentHour(playerSaveData.currentHour);
            SetCurrentMinute(playerSaveData.currentMinute);
        }

        RaiseOnHourChanged();
        RaiseOnMinuteChanged();

        StartCoroutine(Tick());
    }

    private void RaiseOnHourChanged()
    {
        if (onHourChanged != null)
            onHourChanged.Raise();
    }

    private void RaiseOnMinuteChanged()
    {
        if (onMinuteChanged != null)
            onMinuteChanged.Raise();
    }

    public int GetCurrentHour() => currentHour.Value;
    public int GetCurrentMinute() => currentMinute.Value;
    public void SetCurrentHour(int newHour)
    {
        currentHour.Value = newHour;
        RaiseOnHourChanged();
    }
    public void SetCurrentMinute(int newMinute)
    {
        currentMinute.Value = newMinute;
        RaiseOnMinuteChanged();
    }

    public void StopDaytime(bool stop) => stopDaytime = stop;

    public float GetDawnStartTime() => dawnStartTime;
    public float GetDawnMidTime() => GetDawnStartTime() + ((GetDawnEndTime() - GetDawnStartTime()) / 2f);
    public float GetDawnEndTime() => dawnEndTime;
    public float GetDuskStartTime() => duskStartTime;
    public float GetDuskMidTime() => GetDuskStartTime() + ((GetDuskEndTime() - GetDuskStartTime()) / 2f);
    public float GetDuskEndTime() => duskEndTime;

    private IEnumerator Tick()
    {
        while (true)
        {
            if (!stopDaytime)
            {
                yield return new WaitForSeconds(gameMinuteInterval);

                currentMinute.Value++;
                RaiseOnMinuteChanged();
                if (currentMinute.Value >= 60)
                {
                    currentMinute.Value = 0;
                    currentHour.Value++;
                    if (currentHour.Value >= 24)
                        currentHour.Value = 0;
                    RaiseOnHourChanged();
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}
