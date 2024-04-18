using System.Collections.Generic;
using UnityEngine;

public class WidgetPlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject widgetPlayerHealthIconPrefab;
    private List<GameObject> widgetPlayerHealthIcons;
    Player owningPlayer;

    private void Awake()
    {
        widgetPlayerHealthIcons = new List<GameObject>();

        owningPlayer = FindAnyObjectByType<Player>();
        if (owningPlayer)
        {
            owningPlayer.Stats.OnMaxHealthChanged += OnPlayerMaxHealthChanged;
            owningPlayer.Stats.OnHealthChange += OnPlayerHealthChanged;
        }
        else
        {
            Player.OnPlayerAwake += UpdateOwningPlayer;
        }
    }

    private void OnDestroy()
    {
        Player.OnPlayerAwake -= UpdateOwningPlayer;
        if (owningPlayer)
        {
            owningPlayer.Stats.OnMaxHealthChanged -= OnPlayerMaxHealthChanged;
            owningPlayer.Stats.OnHealthChange -= OnPlayerHealthChanged;
        }
    }

    private void UpdateOwningPlayer(Player player)
    {
        owningPlayer = player;
        if (owningPlayer)
        {
            owningPlayer.Stats.OnMaxHealthChanged += OnPlayerMaxHealthChanged;
            owningPlayer.Stats.OnHealthChange += OnPlayerHealthChanged;
        }
    }

    private void OnPlayerHealthChanged(int value)
    {
        for (int i = 1; i <= widgetPlayerHealthIcons.Count; i++)
        {
            WidgetPlayerHealthIcon widgetPlayerHealthIcon = widgetPlayerHealthIcons[i - 1].GetComponent<WidgetPlayerHealthIcon>();
            if (!widgetPlayerHealthIcon)
                continue;

            if (i <= value)
                widgetPlayerHealthIcon.FillItem();
            else
                widgetPlayerHealthIcon.DepleteItem();
        }
    }

    private void OnPlayerMaxHealthChanged(int value)
    {
        for (int i = 1; i <= value; i++)
        {
            if (i >= widgetPlayerHealthIcons.Count)
                widgetPlayerHealthIcons.Add(Instantiate(widgetPlayerHealthIconPrefab, transform));
        }
    }
}
