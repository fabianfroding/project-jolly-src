using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    static string PlayerSaveDataFileName() => "/playerSaveData";

    static string SaveDataFileExtension => ".sun";
    static string SaveDataPath(string fileName) => Application.persistentDataPath + "/Saved" + fileName + SaveDataFileExtension;

    public static void SavePlayerSaveData(PlayerPawn playerPawn, string sceneName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SaveDataPath(PlayerSaveDataFileName()), FileMode.Create);
        PlayerSaveData playerSaveData = new PlayerSaveData(playerPawn, sceneName);
        formatter.Serialize(stream, playerSaveData);
        stream.Close();
    }

    public static PlayerSaveData LoadPlayerSaveData()
    {
        string path = SaveDataPath(PlayerSaveDataFileName());
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SaveDataPath(PlayerSaveDataFileName()), FileMode.Open);
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
}
