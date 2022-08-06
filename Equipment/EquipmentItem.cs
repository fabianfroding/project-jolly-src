using UnityEngine;

public class EquipmentItem : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public bool obtained;
    public bool equipped;
    int id;

    public GameObject originalSlot;
}
