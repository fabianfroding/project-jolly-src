using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINPCNotebook : UIEntityList
{
    [SerializeField] private List<NPCNotebookEntry> list;

    [Tooltip("For NPC-entries that have not yet been encountered.")]
    [SerializeField] private NPCNotebookEntry unknownEntry;
    [Tooltip("For slots outside the array bounds.")]
    [SerializeField] private NPCNotebookEntry emptyEntry;

    [SerializeField] private TextMeshProUGUI currentSlotName;
    [SerializeField] private TextMeshProUGUI currentSlotDescription;

    [SerializeField] private GameObject[] uiNPCNotebooksEntrySlots;

    private void Awake()
    {
        // Add 2 empty slots before the first actual entry.
        for (int i = 0; i < 2; i++)
        {
            list.Insert(i, emptyEntry);
        }

        // Add 3 empty slots efter the last actual entry.
        for (int i = 0; i < 3; i++)
        {
            list.Add(emptyEntry);
        }
    }

    public override void UpdateListView()
    {
        for (int i = 0; i < uiNPCNotebooksEntrySlots.Length; i++)
        {
            NPCNotebookEntry entry = emptyEntry;
            if (NPCNotebookRepository.IsNPCRegisteredInNPCNotebook(list[i + currentIndex].npcName))
            {
                entry = list[i + currentIndex];
            }
            else if (list[i + currentIndex] != emptyEntry)
            {
                entry = unknownEntry;
            }
            uiNPCNotebooksEntrySlots[i].GetComponent<UINPCNotebookEntry>().SetEntry(entry);

            if (i == previewIndex)
            {
                currentSlotName.text = entry.npcName;
                currentSlotDescription.text = entry.description;
            }
        }
    }

    public override void MoveSelectionDown()
    {
        if (!isMoving && currentIndex < list.Count - uiNPCNotebooksEntrySlots.Length)
            currentIndex++;
        base.MoveSelectionDown();
    }
}
