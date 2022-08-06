using UnityEngine;

public class NPCNotebookRepository
{
    private static string NPC_NOTEBOOK_REGISTERED_KEY(string npcName) =>
        ProfileRepository.GetCurrentProfileKey() + npcName.Replace(" ", "") + "NPCNotebookRegistered";

    public static void SaveNPCToNPCNotebook(string npcName) => 
        PlayerPrefs.SetInt(NPC_NOTEBOOK_REGISTERED_KEY(npcName), 1);

    public static bool IsNPCRegisteredInNPCNotebook(string npcName)
    {
        string key = NPC_NOTEBOOK_REGISTERED_KEY(npcName);
        return PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) != 0;
    }
}
