using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private int currentHour = 6;

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

    void Start()
    {
        gameHourInterval = realMinutesPerDay / 24.0f * 60.0f;
        gameMinuteInterval = gameHourInterval / 60f;
        timeAtLastHour = Time.time;

        OnHourChange?.Invoke(currentHour);

        StartCoroutine(Tick());
    }

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
