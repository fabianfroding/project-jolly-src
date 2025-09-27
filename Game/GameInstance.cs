using System.Threading.Tasks;
using UnityEngine;

public class GameInstance : Manager<GameInstance>
{
    [Header("Object Bindings")]
    [SerializeField] private GameObject updateManager;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject defaultLevel;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    
    private GameObject currentlyLoadedLevel;
    
    private async void Start()
    {
        // 1. Bind objects.
        BindObjects();
        // TODO: Show loading screen. Can be updated in between each await to progress the progress bar etc.

        // 2. Initialization - turn on services.

        // 3. Creation.
        await CreateGameObjects();

        // 4. Preparation.
        //await PrepareGame();
        // TODO: Hide loading screen.

        // 5. Begin game.
        //BeginGame();
    }
    
    private void BindObjects()
    {
        if (updateManager)      updateManager = Instantiate(updateManager, gameObject.transform);
    }
    
    private async Task CreateGameObjects()
    {
        if (defaultLevel)
        {
            currentlyLoadedLevel = Instantiate(defaultLevel);
        }
        
        if (player)             player = Instantiate(player, gameObject.transform);
        if (mainCamera)         mainCamera = Instantiate(mainCamera, gameObject.transform);
        await Task.Delay(500);
    }
}
