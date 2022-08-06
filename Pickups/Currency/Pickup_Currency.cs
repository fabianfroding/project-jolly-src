using System;
using UnityEngine;

public class Pickup_Currency : Pickup
{
    [Range(1, 10)]
    [SerializeField] private int currencyAmount;

    protected override void GetPickup()
    {
        base.GetPickup();
        CurrencyManager.Instance.IncreaseCurrency(currencyAmount);
    }
}
