using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerMana : MonoBehaviour
{
    [SerializeField] private Slider widgetPlayerManaFill;
    [SerializeField] private GameObject widgetPlayerManaChargeIconPrefab;
    private List<GameObject> widgetPlayerManaChargeIcons;

    private void Awake()
    {
        widgetPlayerManaChargeIcons = new List<GameObject>();
        StatsPlayer.OnPlayerManaChange += OnPlayerManaChanged;
        StatsPlayer.OnPlayerManaChargesChange += OnPlayerManaChargesChanged;
        StatsPlayer.OnPlayerMaxManaChargesChange += OnPlayerMaxManaChargesChanged;
    }

    private void OnDestroy()
    {
        StatsPlayer.OnPlayerManaChange -= OnPlayerManaChanged;
        StatsPlayer.OnPlayerManaChargesChange -= OnPlayerManaChargesChanged;
        StatsPlayer.OnPlayerMaxManaChargesChange -= OnPlayerMaxManaChargesChanged;
    }

    private void OnPlayerManaChanged(int value)
    {
        if (widgetPlayerManaFill)
            StartCoroutine(SmoothSliderValueChange(value));
    }

    private void OnPlayerManaChargesChanged(int value)
    {
        for (int i = 1; i <= widgetPlayerManaChargeIcons.Count; i++)
        {
            WidgetPlayerManaChargeIcon widgetPlayerManaChargeIcon = widgetPlayerManaChargeIcons[i - 1].GetComponent<WidgetPlayerManaChargeIcon>();
            if (!widgetPlayerManaChargeIcon)
                continue;

            if (i <= value)
                widgetPlayerManaChargeIcon.FillItem();
            else
                widgetPlayerManaChargeIcon.DepleteItem();
        }
    }

    private void OnPlayerMaxManaChargesChanged(int value)
    {
        for (int i = 1; i <= value; i++)
        {
            if (i >= widgetPlayerManaChargeIcons.Count)
                widgetPlayerManaChargeIcons.Add(Instantiate(widgetPlayerManaChargeIconPrefab, transform));
        }
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
}
