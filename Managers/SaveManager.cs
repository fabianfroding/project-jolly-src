using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// TODO: This need to be reworked to have multiple files for different save data to avoid excessive parameters for save functions.
public static class SaveManager
{
    public static int currentSaveSlotIndex;
    static string PlayerSaveDataFileName() => "/playerSaveData" + currentSaveSlotIndex;
    static string PickupablesSaveDataFileName() => "/pickupablesSaveData" + currentSaveSlotIndex;
    static string UnlockablePlayerAbilitiesSaveDataFileName() => "/unlockablePlayerAbilitiesSaveData" + currentSaveSlotIndex;

    static string SaveDataFileExtension => ".dat";
    static string SaveDataPath(string fileName) => Application.persistentDataPath + fileName + SaveDataFileExtension;

    public static event Action OnGameSaved;

    public static void SavePlayerSaveData(PlayerCharacter playerCharacter, Vector2 position, string sceneName)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(SaveDataPath(PlayerSaveDataFileName()), FileMode.Create);
        PlayerSaveData playerSaveData = new(playerCharacter, position.x, position.y, sceneName);

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
        path = SaveDataPath(UnlockablePlayerAbilitiesSaveDataFileName());
        if (File.Exists(path))
            File.Delete(path);
    }

    public static bool DoesPlayerSaveDataExist() => File.Exists(SaveDataPath(PlayerSaveDataFileName()));

    #region Pickupables Data
    public static bool HasPickupableBeenPickedUp(string pickupableKey) => LoadPickupablesData().Contains(pickupableKey);

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

    #region Unlockable Player Abilties
    public static bool IsUnlockablePlayerAbilityUnlocked(Types.EUnlockablePlayerAbilityID unlockablePlayerAbilityID) =>
        LoadUnlockedPlayerAbilities().Contains(unlockablePlayerAbilityID);

    public static List<Types.EUnlockablePlayerAbilityID> LoadUnlockedPlayerAbilities()
    {
        string path = SaveDataPath(UnlockablePlayerAbilitiesSaveDataFileName());
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);
            List<Types.EUnlockablePlayerAbilityID> unlockedPlayerAbilities = 
                formatter.Deserialize(stream) as List<Types.EUnlockablePlayerAbilityID>;
            stream.Close();
            return unlockedPlayerAbilities;
        }
        return new List<Types.EUnlockablePlayerAbilityID>();
    }

    public static void SaveUnlockedPlayerAbilityID(Types.EUnlockablePlayerAbilityID unlockablePlayerAbilityID)
    {
        List<Types.EUnlockablePlayerAbilityID> unlockedPlayerAbilities = LoadUnlockedPlayerAbilities();
        if (!unlockedPlayerAbilities.Contains(unlockablePlayerAbilityID))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(SaveDataPath(UnlockablePlayerAbilitiesSaveDataFileName()), FileMode.Create);
            unlockedPlayerAbilities.Add(unlockablePlayerAbilityID);
            formatter.Serialize(stream, unlockedPlayerAbilities);
            stream.Close();
            OnGameSaved?.Invoke();
        }
    }
    #endregion
}
