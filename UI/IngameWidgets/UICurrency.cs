using TMPro;
using UnityEngine;

public class UICurrency : UIPopupText
{
    [SerializeField] private TextMeshProUGUI currencyAmountText;

    private static UICurrency instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    protected override void Start()
    {
        UpdateUI(CurrencyManager.Currency);

        StopCoroutine(DestroyAfterDelay());
        StartCoroutine(DestroyAfterDelay());
    }

    private void UpdateUI(int amount) => currencyAmountText.text = amount.ToString();
}
