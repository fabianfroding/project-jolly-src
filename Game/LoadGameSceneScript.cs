using UnityEngine;

// This script acts as a one-time executioner for data that should only be loaded once upon starting/loading the game.
public class LoadGameSceneScript : MonoBehaviour
{
    public static readonly string LOAD_GAME_SCENE_NAME = "LoadGameScene";

    private void Awake()
    {
        Debug.Log("Loading potential save data.");
        LoadCurrencyData();
        StartGame();
    }

    public static void StartGame() =>
        ProfileManager.LoadSceneForCurrentProfile();

    private void LoadCurrencyData()
    {
        GameObject tempGO = new GameObject("CurrencyManager");
        tempGO.transform.parent = transform;
        tempGO.AddComponent<CurrencyManager>();
        CurrencyManager.Instance.LoadCurrency();
        CurrencyManager.Instance.LoadCurrencyCap();
    }
}
