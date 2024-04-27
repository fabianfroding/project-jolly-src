using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private SODaytimeSettings daytimeSettings;
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

    private bool stopDaytime = false;

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

    public int GetCurrentHour() => daytimeSettings.currentHour;
    public int GetCurrentMinute() => daytimeSettings.currentMinute;
    public void SetCurrentHour(int newHour)
    {
        daytimeSettings.currentHour = newHour;
        RaiseOnHourChanged();
    }
    public void SetCurrentMinute(int newMinute)
    {
        daytimeSettings.currentMinute = newMinute;
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

                daytimeSettings.currentMinute++;
                RaiseOnMinuteChanged();
                if (daytimeSettings.currentMinute >= 60)
                {
                    daytimeSettings.currentMinute = 0;
                    daytimeSettings.currentHour++;
                    if (daytimeSettings.currentHour >= 24)
                        daytimeSettings.currentHour = 0;
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
