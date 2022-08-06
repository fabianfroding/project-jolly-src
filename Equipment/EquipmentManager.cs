using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    /* This is static so that it will "remember" equipped items when toggling the equipment menu. */
    private static EquipmentItem[] equippedItems;
    [SerializeField] private EquipmentItem[] items;

    public static event Action OnEquipItem;

    public void EquipItem()
    {
        OnEquipItem?.Invoke();
    }

    public void Add(EquipmentItem e, int index)
    {
        if (equippedItems == null)
            equippedItems = new EquipmentItem[6];

        equippedItems[index] = e;
    }
}
