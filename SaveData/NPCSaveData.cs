using System;

[Serializable]
public class NPCSaveData
{
    public int dialogueIndex;

    public NPCSaveData(NPCPawn npcPawn)
    {
        dialogueIndex = npcPawn.GetInteractDataIndex();
    }
}
