using UnityEngine;

[CreateAssetMenu(fileName = "newNPCNotebookEntry", menuName = "Data/NPCNotebookData/NPCNotebookEntry")]
public class NPCNotebookEntry : ScriptableObject
{
    public string npcName;
    [TextArea] public string description;

    public Sprite sprite;
}
