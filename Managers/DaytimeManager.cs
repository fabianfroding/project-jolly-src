using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private int currentHour = 6;

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
    private float timeAtLastHour = 0f;

    public bool stopDaytime = false;

    public static event Action<int> OnHourChange;
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
        timeAtLastHour = Time.time;

        OnHourChange?.Invoke(currentHour);

        StartCoroutine(Tick());
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

                if (Time.time >= timeAtLastHour + gameHourInterval)
                {   
                    currentHour++;
                    timeAtLastHour = Time.time;
                    if (currentHour >= 24)
                        currentHour = 0;
                    OnHourChange?.Invoke(currentHour);
                }

                float tickTime = currentHour + ((Time.time - timeAtLastHour) / gameHourInterval);
                OnDaytimeTick?.Invoke(tickTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}
