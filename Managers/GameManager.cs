using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject widgetHUD;
    [SerializeField] private float playerReviveDelay = 2.5f;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private SODaytimeSettings daytimeSettings;

    public static event Action OnGameOver;
    public static event Action OnRevivePlayer;

    private void Awake()
    {
        GameManager[] gameManagers = GameObject.FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        AddWidgetHUD();
        Cursor.visible = false;

        PlayerPawn.OnPlayerDeathSequenceFinish += OnPlayerDeathSequenceFinish;
        PlayerPawn.OnQuitInput += QuitToMainMenu;
    }

    private void OnDestroy()
    {
        PlayerPawn.OnPlayerDeathSequenceFinish -= OnPlayerDeathSequenceFinish;
        PlayerPawn.OnQuitInput -= QuitToMainMenu;
    }

    private void AddWidgetHUD()
    {
        GameObject tempGO = Instantiate(widgetHUD);
        tempGO.transform.SetParent(transform);
    }

    private void OnPlayerDeathSequenceFinish(PlayerPawn playerPawn)
    {
        OnGameOver?.Invoke();
        StartCoroutine(RevivePlayer(playerPawn));
    }

    private IEnumerator RevivePlayer(PlayerPawn playerPawn)
    {
        yield return new WaitForSeconds(playerReviveDelay);

        if (SaveManager.DoesPlayerSaveDataExist() && playerPawn)
        {
            PlayerSaveData playerSaveData = SaveManager.LoadPlayerSaveData();

            if (SceneManager.GetActiveScene().name != playerSaveData.sceneName)
                SceneManager.LoadScene(playerSaveData.sceneName);

            playerPawn.transform.position = new(playerSaveData.position[0], playerSaveData.position[1]);
            playerPawn.HealthComponent.SetMaxHealth(playerSaveData.playerMaxHealth);
            playerPawn.HealthComponent.SetHealth(playerSaveData.playerHealth);
            playerPawn.Revive();
            OnRevivePlayer?.Invoke();

            if (daytimeSettings)
            {
                daytimeSettings.currentHour = playerSaveData.currentHour;
                daytimeSettings.currentMinute = playerSaveData.currentMinute;
            }
        }
        else
        {
            QuitToMainMenu();
        }
    }

    private void QuitToMainMenu()
    {
        PlayerPawn playerPawn = FindObjectOfType<PlayerPawn>();
        if (playerPawn)
        {
            Destroy(playerPawn.gameObject);
        }
        SceneManager.LoadScene(mainMenuSceneName);
        Destroy(gameObject);
    }
}
