using System;
using System.Collections;
using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private float realMinutesPerDay = 0.04f;
    [SerializeField] private float playerMaxVisibilityRadius = 48f;
    [SerializeField] private float playerMinVisibilityRadius = 12f;

    private float gameHourInterval;
    private int currentHour = 6;
    private Player player;

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
        Debug.Log(gameHourInterval);

        OnHourChange?.Invoke(currentHour);
        UpdatePlayerVisibilityRadius(currentHour);

        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            // TODO: Change this back to update in game-minutes to make the player visibility radius smoother.
            // TODO: Pause the coroutine while player is interacting.
            yield return new WaitForSeconds(gameHourInterval);
            currentHour++;
            if (currentHour >= 24)
                currentHour = 0;
            OnHourChange?.Invoke(currentHour);

            UpdatePlayerVisibilityRadius(currentHour);
        }
    }

    private void UpdatePlayerVisibilityRadius(int hour)
    {
        if (!player)
            player = FindObjectOfType<Player>();

        if (!player)
        {
            Debug.LogWarning("DaytimeManager::UpdatePlayerVisibilityRadius: Unable to find player.");
            return;
        }

        float hoursAwayFromMidday = Mathf.Abs(hour - 12);
        player.SetVisibilityRadius(playerMaxVisibilityRadius - (hoursAwayFromMidday * ((playerMaxVisibilityRadius - playerMinVisibilityRadius) / 12)));
    }
}
