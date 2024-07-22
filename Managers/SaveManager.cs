using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// TODO: This need to be reworked to have multiple files for different save data to avoid excessive parameters for save functions.
public static class SaveManager
{
    public static int currentSaveSlotIndex;
    static string PlayerSaveDataFileName() => "/playerSaveData" + currentSaveSlotIndex;
    static string PickupablesSaveDataFileName() => "/pickupablesSaveData" + currentSaveSlotIndex;

    static string SaveDataFileExtension => ".dat";
    static string SaveDataPath(string fileName) => Application.persistentDataPath + fileName + SaveDataFileExtension;

    public static event Action OnGameSaved;

    public static void SavePlayerSaveData(PlayerPawn playerPawn, Vector2 position, string sceneName, SODaytimeSettings daytimeSettings)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(SaveDataPath(PlayerSaveDataFileName()), FileMode.Create);
        PlayerSaveData playerSaveData = new(playerPawn, position.x, position.y, sceneName);

        if (daytimeSettings)
        {
            playerSaveData.currentHour = daytimeSettings.currentHour;
            playerSaveData.currentMinute = daytimeSettings.currentMinute;
        }

        formatter.Serialize(stream, playerSaveData);
        stream.Close();

        OnGameSaved?.Invoke();
    }

    public static PlayerSaveData LoadPlayerSaveData()
    {
        string path = SaveDataPath(PlayerSaveDataFileName());
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(SaveDataPath(PlayerSaveDataFileName()), FileMode.Open);
            PlayerSaveData playerSaveData = formatter.Deserialize(stream) as PlayerSaveData;
            stream.Close();
            return playerSaveData;
        }
        else
        {
            Debug.LogError("SaveManager::LoadPlayerSaveData: Could not find save data in " + path);
        }
        return null;
    }

    public static void ClearPlayerSaveData()
    {
        string path = SaveDataPath(PlayerSaveDataFileName());
        if (File.Exists(path))
            File.Delete(path);
        path = SaveDataPath(PickupablesSaveDataFileName());
        if (File.Exists(path))
            File.Delete(path);
    }

    public static bool DoesPlayerSaveDataExist() => File.Exists(SaveDataPath(PlayerSaveDataFileName()));

    #region Pickupables Data
    public static List<string> LoadPickupablesData()
    {
        string path = SaveDataPath(PickupablesSaveDataFileName());
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);
            List<string> pickupablesData = formatter.Deserialize(stream) as List<string>;
            stream.Close();
            return pickupablesData;
        }
        return new List<string>();
    }

    public static bool HasPickupableBeenPickedUp(string pickupableKey) => LoadPickupablesData().Contains(pickupableKey);

    public static void SavePickupable(string pickupableKey)
    {
        List<string> pickupablesData = LoadPickupablesData();
        if (!pickupablesData.Contains(pickupableKey))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(SaveDataPath(PickupablesSaveDataFileName()), FileMode.Create);
            pickupablesData.Add(pickupableKey);
            formatter.Serialize(stream, pickupablesData);
            stream.Close();
            OnGameSaved?.Invoke();
        }
    }
    #endregion
}
