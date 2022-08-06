using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyListEntry", menuName = "Data/EnemyListData/EnemyEntry")]
public class EnemyListEntry : ScriptableObject
{
    public string enemyName;
    [TextArea]
    public string description;

    [Tooltip("The sprite displayed when viewing the enemy entry.")]
    public Sprite entryViewSprite;
    public Sprite shinyEntryViewSprite;
    [Tooltip("The sprite displayed in the list as an icon/preview.")]
    public Sprite listPreviewSprite;

    public int numKillsRequired;
}
