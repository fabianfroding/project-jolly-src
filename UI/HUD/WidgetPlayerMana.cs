using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerMana : MonoBehaviour
{
    [SerializeField] private Slider widgetPlayerManaFill;
    [SerializeField] private GameObject widgetPlayerManaChargeIconPrefab;

    [SerializeField] private SOIntVariable playerMana;
    [SerializeField] private SOIntVariable playerManaCharges;
    [SerializeField] private SOIntVariable playerMaxManaCharges;

    private List<GameObject> widgetPlayerManaChargeIcons;

    private void Awake()
    {
        widgetPlayerManaChargeIcons = new List<GameObject>();
    }

    private IEnumerator SmoothSliderValueChange(float targetValue)
    {
        float currentValue = widgetPlayerManaFill.value;
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            widgetPlayerManaFill.value = Mathf.Lerp(currentValue, targetValue, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        widgetPlayerManaFill.value = targetValue;
    }

    public void OnPlayerManaChanged()
    {
        if (isActiveAndEnabled && widgetPlayerManaFill)
            StartCoroutine(SmoothSliderValueChange(playerMana.Value));
    }

    public void OnPlayerManaChargesChanged()
    {
        for (int i = 1; i <= widgetPlayerManaChargeIcons.Count; i++)
        {
            WidgetPlayerManaChargeIcon widgetPlayerManaChargeIcon = widgetPlayerManaChargeIcons[i - 1].GetComponent<WidgetPlayerManaChargeIcon>();
            if (!widgetPlayerManaChargeIcon)
                continue;

            if (i <= playerManaCharges.Value)
                widgetPlayerManaChargeIcon.FillItem();
            else
                widgetPlayerManaChargeIcon.DepleteItem();
        }
    }

    public void OnPlayerMaxManaChargesChanged()
    {
        for (int i = 1; i <= playerMaxManaCharges.Value; i++)
        {
            if (i >= widgetPlayerManaChargeIcons.Count)
                widgetPlayerManaChargeIcons.Add(Instantiate(widgetPlayerManaChargeIconPrefab, transform));
        }
    }
}
