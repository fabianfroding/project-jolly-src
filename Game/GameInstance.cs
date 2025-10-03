using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : Manager<GameInstance>
{
    [SerializeField] private string mainMenuScene = "MainMenuScene";
    
    [Header("Object Bindings")]
    [SerializeField] private GameObject updateManager;

    [SerializeField] private GameObject widgetManager;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject defaultLevel;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    
    private GameObject currentlyLoadedLevel;
    private WidgetGameOverlay widgetGameOverlay;
    
    private async void Start()
    {
        // 1. Bind objects.
        BindObjects();
        // TODO: Show loading screen. Can be updated in between each await to progress the progress bar etc.

        // 2. Initialization - turn on services.

        // 3. Creation.
        await CreateGameObjects();

        // 4. Preparation.
        await PrepareGame();
        // TODO: Hide loading screen.

        // 5. Begin game.
        BeginGame();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<LevelChangeEvent>(HandleLevelChangedEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<LevelChangeEvent>(HandleLevelChangedEvent);
    }

    public void GoToMainMenu()
    {
        EventBus.Clear();
        UpdateManager.ClearUpdateManager();
        SceneManager.LoadScene(mainMenuScene);
    }
    
    private void BindObjects()
    {
        if (updateManager)      updateManager = Instantiate(updateManager, gameObject.transform);
        if (widgetManager)
        {
            widgetManager = Instantiate(widgetManager, gameObject.transform);
            
            // TEMP. Needs refactor.
            WidgetManager manager = widgetManager.GetComponent<WidgetManager>();
            if (manager)
            {
                GameObject gameOverlay = manager.GetWidgetGameOverlayGO();
                if (gameOverlay)
                {
                    widgetGameOverlay = gameOverlay.GetComponent<WidgetGameOverlay>();
                }
            }
        }
    }
    
    private async Task CreateGameObjects()
    {
        if (defaultLevel)
        {
            currentlyLoadedLevel = Instantiate(defaultLevel);
        }
        
        if (player)
        {
            player = Instantiate(player, gameObject.transform);
            player.gameObject.SetActive(false);
        }
        
        if (mainCamera)
        {
            mainCamera = Instantiate(mainCamera, gameObject.transform);
        }
        
        await Task.Delay(500);
    }
    
    private async Task PrepareGame()
    {
        // Move player, give startup equipment, move enemies, update UI etc.
        
        if (player)
        {
            PlayerStart.InitializePlayerLocation(player);
        }

        await Task.Delay(500);
    }
    
    private async void BeginGame()
    {
        // Await UI animations to finish, then enable input, show & turn on enemies etc.
        
        if (player)
        {
            player.gameObject.SetActive(true);
        }

        await Task.Delay(500);
    }
    
    private async void HandleLevelChangedEvent(LevelChangeEvent levelChangedEvent)
    {
        GameObject levelToLoad = levelChangedEvent.GetLevelDataToLoad().level;
        
        GameObject previousLevelGO = currentlyLoadedLevel;
        Level previousLevel = previousLevelGO.GetComponent<Level>();
        LevelData previousLevelData = previousLevel.GetLevelData();

        PlayerCharacter playerCharacter = player.GetComponent<PlayerCharacter>();
        if (playerCharacter)
        {
            //playerCharacter.PausePlayerCharacter(true);
        }

        Time.timeScale = 0;
        if (widgetGameOverlay)
            await widgetGameOverlay.FadeOut();

        await UnloadCurrentLevel();
        await LoadNewLevel(levelToLoad);

        Level loadedlevel = currentlyLoadedLevel.GetComponent<Level>();
        if (loadedlevel != null)
        {
            loadedlevel.InitializePlayerAtEntryPoint(player, previousLevelData);
        }

        CameraScript.SnapToPlayer(player);

        if (widgetGameOverlay)
            widgetGameOverlay.FadeInFast();

        if (playerCharacter)
        {
            //playerCharacter.PausePlayerCharacter(false);
        }
        Time.timeScale = 1;
    }
    
    private async Task UnloadCurrentLevel()
    {
        if (currentlyLoadedLevel)
        {
            Destroy(currentlyLoadedLevel);
        }
        await Task.Delay(500);
    }

    private async Task LoadNewLevel(GameObject levelToLoad)
    {
        if (levelToLoad)
        {
            currentlyLoadedLevel = Instantiate(levelToLoad);
        }
        await Task.Delay(500);
    }
}
