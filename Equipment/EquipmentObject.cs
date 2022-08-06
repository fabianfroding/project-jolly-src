using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentItem", menuName = "Data/EquipmentItemData/EquipmentItem")]
public class EquipmentObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public bool obtained = false;
    public bool equipped;
    int id;
}
