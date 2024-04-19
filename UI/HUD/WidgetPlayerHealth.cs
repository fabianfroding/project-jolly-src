using System.Collections.Generic;
using UnityEngine;

public class WidgetPlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject widgetPlayerHealthIconPrefab;
    private List<GameObject> widgetPlayerHealthIcons;
    PlayerPawn owningPlayer;

    private void Awake()
    {
        widgetPlayerHealthIcons = new List<GameObject>();
    }

    private void Start()
    {
        owningPlayer = FindAnyObjectByType<PlayerPawn>();
        if (owningPlayer && owningPlayer.HealthComponent)
        {
            owningPlayer.HealthComponent.OnMaxHealthChanged += OnPlayerMaxHealthChanged;
            owningPlayer.HealthComponent.OnHealthChange += OnPlayerHealthChanged;
        }
        else
        {
            PlayerPawn.OnPlayerAwake += UpdateOwningPlayer;
        }
    }

    private void OnDestroy()
    {
        PlayerPawn.OnPlayerAwake -= UpdateOwningPlayer;
        if (owningPlayer && owningPlayer.HealthComponent)
        {
            owningPlayer.HealthComponent.OnMaxHealthChanged -= OnPlayerMaxHealthChanged;
            owningPlayer.HealthComponent.OnHealthChange -= OnPlayerHealthChanged;
        }
    }

    private void UpdateOwningPlayer(PlayerPawn player)
    {
        owningPlayer = player;
        if (owningPlayer && owningPlayer.HealthComponent)
        {
            owningPlayer.HealthComponent.OnMaxHealthChanged += OnPlayerMaxHealthChanged;
            owningPlayer.HealthComponent.OnHealthChange += OnPlayerHealthChanged;
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
