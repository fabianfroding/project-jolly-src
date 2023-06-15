using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyList : UIEntityList
{
    [SerializeField] private List<EnemyListEntry> list;

    [Tooltip("For enemy-entries that have not yet been encountered.")]
    [SerializeField] private EnemyListEntry unknownEntry;
    [Tooltip("For enemy-entries that have been encountered but without the required amount of kills for details.")]
    [SerializeField] private EnemyListEntry incompleteEntry;
    [Tooltip("For slots outside the array bounds.")]
    [SerializeField] private EnemyListEntry emptyEntry;

    [SerializeField] private TextMeshProUGUI currentSlotName;
    [SerializeField] private Image currentSlotSprite;
    [SerializeField] private Image currentSlotSpriteVariant;
    [SerializeField] private TextMeshProUGUI currentSlotDescription;

    [SerializeField] private GameObject[] uiEnemyListEntrySlots;

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
        for (int i = 0; i < uiEnemyListEntrySlots.Length; i++)
        {
            int numKilled = EnemyListRepository.LoadEnemyListNumKilled(list[i + currentIndex].enemyName);
            int numKillsRequired = list[i + currentIndex].numKillsRequired;

            EnemyListEntry entry = unknownEntry;
            if (numKilled > 0 && numKilled < numKillsRequired)
            {
                incompleteEntry.enemyName = list[i + currentIndex].enemyName;
                incompleteEntry.description = GetIncompleteEntryDescription(numKillsRequired - numKilled);
                incompleteEntry.entryViewSprite = list[i + currentIndex].entryViewSprite;

                // Check if shiny variant has been killed.
                if (EnemyListRepository.LoadEnemyListNumKilled(ShinyEnemyRandomizer.SHINY_ENEMY_NAME_PREFIX + list[i + currentIndex].enemyName) > 0)
                {
                    incompleteEntry.shinyEntryViewSprite = list[i + currentIndex].shinyEntryViewSprite;
                }
                else
                {
                    incompleteEntry.shinyEntryViewSprite = unknownEntry.shinyEntryViewSprite;
                }

                incompleteEntry.listPreviewSprite = list[i + currentIndex].listPreviewSprite;
                incompleteEntry.numKillsRequired = numKillsRequired;
                entry = incompleteEntry;
            }
            else if (numKilled >= numKillsRequired)
            {
                entry = list[i + currentIndex];
            }
            uiEnemyListEntrySlots[i].GetComponent<UIEnemyListEntry>().SetEntry(entry);

            if (i == previewIndex)
            {
                currentSlotName.text = entry.enemyName;
                currentSlotSprite.sprite = entry.entryViewSprite;
                currentSlotSpriteVariant.sprite = entry.shinyEntryViewSprite;
                currentSlotDescription.text = entry.description;

                currentSlotSprite.preserveAspect = true;
                currentSlotSpriteVariant.preserveAspect = true;
            }
        }
    }

    private string GetIncompleteEntryDescription(int killsLeft) => 
        "Defeat " + killsLeft + " more to complete the entry.";

    public override void MoveSelectionDown()
    {
        if (!isMoving && currentIndex < list.Count - uiEnemyListEntrySlots.Length)
            currentIndex++;
        base.MoveSelectionDown();
    }
}
