using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SaveManager>();
            return instance;
        }
    }
    
    public static event Action OnGameSave;

    private void Awake()
    {
        RevivePoint.OnRevivePointSave += SaveGame;
    }

    private void OnDestroy()
    {
        RevivePoint.OnRevivePointSave -= SaveGame;
    }

    private void SaveGame()
    {
        PlayerPawn player = FindObjectOfType<PlayerPawn>();
        

        OnGameSave?.Invoke();
    }
}
