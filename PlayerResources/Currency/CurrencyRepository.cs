using UnityEngine;

public class CurrencyRepository
{
    private static string CURRENCY_KEY() => ProfileRepository.GetCurrentProfileKey() + "Currency";
    private static string CURRENCY_CAP_KEY() => ProfileRepository.GetCurrentProfileKey() + "CurrencyCap";

    public static int Currency
    {
        get => PlayerPrefs.GetInt(CURRENCY_KEY());
        set => PlayerPrefs.SetInt(CURRENCY_KEY(), value);
    }

    public static int CurrencyCap
    {
        get => PlayerPrefs.GetInt(CURRENCY_CAP_KEY());
        set => PlayerPrefs.SetInt(CURRENCY_CAP_KEY(), value);
    }

    public static bool HasCurrencyKey() => PlayerPrefs.HasKey(CURRENCY_KEY());
    public static bool HasCurrencyCapKey() => PlayerPrefs.HasKey(CURRENCY_CAP_KEY());
}
