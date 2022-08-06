using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIEquipmentSlot : MonoBehaviour
{
    public GameObject equipmentItemPrefab;
    public EquipmentItem EquipmentItem;
    public Image image;
    private Sprite defaultSprite;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }

    private void Start()
    {
        if (equipmentItemPrefab != null)
        {
            EquipmentItem = equipmentItemPrefab.GetComponent<EquipmentItem>();
            if (EquipmentItem.obtained)
            {
                image.sprite = EquipmentItem.itemSprite;
                image.color = Color.white;
            }
            else
            {
                image.sprite = defaultSprite;
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }
}
