using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private int currentHour = 6;
    [SerializeField] private int currentMinute = 0;

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

    public static event Action<int, int> OnTimeChange;
    public static event Action<float> OnDaytimeTick;

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

        OnTimeChange?.Invoke(currentHour, currentMinute);

        StartCoroutine(Tick());
    }

    public int GetCurrentHour() => currentHour;
    public int GetCurrentMinute() => currentMinute;
    public void SetCurrentHour(int newHour)
    {
        currentHour = newHour;
        OnTimeChange?.Invoke(currentHour, currentMinute);
    }
    public void SetCurrentMinute(int newMinute)
    {
        currentMinute = newMinute;
        OnTimeChange?.Invoke(currentHour, currentMinute);
    }

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

                currentMinute++;
                if (currentMinute >= 60)
                {
                    currentMinute = 0;
                    currentHour++;
                    if (currentHour >= 24)
                        currentHour = 0;
                }

                float tickTime = currentHour + (currentMinute / 60f);
                OnDaytimeTick?.Invoke(tickTime);
                OnTimeChange?.Invoke(currentHour, currentMinute);
            }
            else
            {
                yield return null;
            }
        }
    }
}
