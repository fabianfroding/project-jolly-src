using System.Collections.Generic;
using UnityEngine;

public class WidgetPlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject widgetPlayerHealthIconPrefab;
    [SerializeField] private SOIntVariable playerHealth;
    [SerializeField] private SOIntVariable playerMaxHealth;
    private List<GameObject> widgetPlayerHealthIcons;

    private void Awake()
    {
        widgetPlayerHealthIcons = new List<GameObject>();
    }

    public void OnPlayerHealthChanged()
    {
        for (int i = 1; i <= widgetPlayerHealthIcons.Count; i++)
        {
            WidgetPlayerHealthIcon widgetPlayerHealthIcon = widgetPlayerHealthIcons[i - 1].GetComponent<WidgetPlayerHealthIcon>();
            if (!widgetPlayerHealthIcon)
                continue;

            if (i <= playerHealth.Value)
                widgetPlayerHealthIcon.FillItem();
            else
                widgetPlayerHealthIcon.DepleteItem();
        }
    }

    public void OnPlayerMaxHealthChanged()
    {
        for (int i = 1; i <= playerMaxHealth.Value; i++)
        {
            if (i >= widgetPlayerHealthIcons.Count + 1)
            {
                widgetPlayerHealthIcons.Add(Instantiate(widgetPlayerHealthIconPrefab, transform));
            }
        }
    }
}
