using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static int Currency { get; private set; }
    public static int CurrencyCap { get; private set; }
    private const int MAX_CURRENCY_CAP = 10000, MIN_CURRENCY_CAP = 100;

    public static event Action OnCurrencyChange;

    public static CurrencyManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CurrencyManager>();
            return instance;
        }
    }
    private static CurrencyManager instance;

    public void IncreaseCurrency(int amount)
    {
        Currency += amount;
        AdjustCurrency();
        InvokeOnCurrencyChangeEvent();
    }

    public void SetCurrency(int amount)
    {
        Currency = amount;
        AdjustCurrency();
        InvokeOnCurrencyChangeEvent();
    }

    private void AdjustCurrency() => Currency = Mathf.Clamp(Currency, 0, CurrencyCap);

    private void InvokeOnCurrencyChangeEvent() => OnCurrencyChange?.Invoke();

    public void SaveCurrency()
    {
        CurrencyRepository.Currency = Currency;
        CurrencyRepository.CurrencyCap = CurrencyCap;
    }

    public void LoadCurrency()
    {
        if (CurrencyRepository.HasCurrencyKey())
        {
            Currency = CurrencyRepository.Currency;
        }
    }

    public void LoadCurrencyCap()
    {
        if (CurrencyRepository.HasCurrencyCapKey())
        {
            CurrencyCap = CurrencyRepository.CurrencyCap;
        }
        else
        {
            CurrencyCap = MIN_CURRENCY_CAP;
        }
    }
}
