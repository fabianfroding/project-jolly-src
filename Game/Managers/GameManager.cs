using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject widgetHUD;
    [SerializeField] private float playerDeathToMainMenuDelay = 2.5f;
    [SerializeField] private string mainMenuSceneName;

    private float onPlayerDeathStartTime = 0f;

    private void Awake()
    {
        GameManager[] gameManagers = GameObject.FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        AddWidgetHUD();

        Player.OnPlayerDeath += OnPlayerDeath;
    }

    private void FixedUpdate()
    {
        if (onPlayerDeathStartTime > 0f && Time.time >= onPlayerDeathStartTime + playerDeathToMainMenuDelay)
            GameOverToMainMenu();
    }

    private void OnDestroy()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
    }

    private void AddWidgetHUD()
    {
        GameObject tempGO = Instantiate(widgetHUD);
        tempGO.transform.SetParent(transform);
    }

    private void OnPlayerDeath()
    {
        onPlayerDeathStartTime = Time.time;
    }

    private void GameOverToMainMenu()
    {
        onPlayerDeathStartTime = 0f;
        SceneManager.LoadScene(mainMenuSceneName);
        Destroy(gameObject);
    }
}
