using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 18f;
    [SerializeField] private float playerMaxVisibilityRadius = 48f;
    [SerializeField] private float playerMinVisibilityRadius = 12f;
    [SerializeField] private int currentHour = 6;

    private float gameHourInterval;
    private float gameMinuteInterval;
    private float timeAtLastHour = 0f;
    private Player player;

    public bool stopDaytime = false;

    public static event Action<int> OnHourChange;

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
        UpdatePlayerVisibilityRadius(currentHour);

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

                UpdatePlayerVisibilityRadius(currentHour + ((Time.time - timeAtLastHour) / gameHourInterval));
            }
            else
            {
                yield return null;
            }
        }
    }

    private void UpdatePlayerVisibilityRadius(float time)
    {
        if (!player)
            player = FindObjectOfType<Player>();

        if (!player)
        {
            Debug.LogWarning("DaytimeManager::UpdatePlayerVisibilityRadius: Unable to find player.");
            return;
        }

        float hoursAwayFromMidday = Mathf.Abs(time - 12);
        player.SetVisibilityRadius(playerMaxVisibilityRadius - (hoursAwayFromMidday * ((playerMaxVisibilityRadius - playerMinVisibilityRadius) / 12)));
    }
}
