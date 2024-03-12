using UnityEngine;

// This script acts as a one-time executioner for data that should only be loaded once upon starting/loading the game.
public class LoadGameSceneScript : MonoBehaviour
{
    public static readonly string LOAD_GAME_SCENE_NAME = "Scene_LoadGame";

    private void Awake()
    {
        Debug.Log("Loading potential save data.");
        StartGame();
    }

    public static void StartGame() =>
        ProfileManager.LoadSceneForCurrentProfile();
}
