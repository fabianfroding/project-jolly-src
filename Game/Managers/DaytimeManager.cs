using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18.0f;
    private float gameHourInterval;
    private int currentHour = 6;

    public event Action<int> OnHourChange;

    void Start()
    {
        gameHourInterval = realMinutesPerDay / 24.0f * 60.0f;
        OnHourChange += OnHourChanged;
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(gameHourInterval);
            currentHour++;
            if (currentHour >= 24)
                currentHour = 0;
            OnHourChange?.Invoke(currentHour);
        }
    }

    private void OnHourChanged(int hour)
    {
        Debug.Log("DaytimeManager::OnHourChanged: Hour " + hour);
    }
}
