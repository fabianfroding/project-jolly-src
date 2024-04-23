using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    static string PlayerSaveDataFileName() => "/playerSaveData";

    static string SaveDataFileExtension => ".sun";
    static string SaveDataPath(string fileName) => Application.persistentDataPath + fileName + SaveDataFileExtension;

    public static event Action OnGameSaved;

    public static void SavePlayerSaveData(PlayerPawn playerPawn, Vector2 position, string sceneName)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(SaveDataPath(PlayerSaveDataFileName()), FileMode.Create);
        PlayerSaveData playerSaveData = new(playerPawn, position.x, position.y, sceneName);
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
    }

    public static bool DoesPlayerSaveDataExist() => File.Exists(SaveDataPath(PlayerSaveDataFileName()));
}
