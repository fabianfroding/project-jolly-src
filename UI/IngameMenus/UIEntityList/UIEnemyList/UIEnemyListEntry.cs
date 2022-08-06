using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyListEntry : MonoBehaviour
{
    [SerializeField] private Image listImage;
    [SerializeField] private TextMeshProUGUI listName;
    private EnemyListEntry entry;

    public void SetEntry(EnemyListEntry entry)
    {
        this.entry = entry;
        listName.text = entry.enemyName;
        listImage.sprite = entry.listPreviewSprite;
    }
}
