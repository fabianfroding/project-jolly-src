using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : UIIngameWidget
{
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite spriteFilled; 
    [SerializeField] private Sprite spriteEmpty;
    private StatsPlayer statsPlayer;

    private void Awake()
    {
        statsPlayer = FindObjectOfType<StatsPlayer>();
        if (!statsPlayer)
        {
            Debug.LogError("UIPlayerHealth:Awake: Could not find object of type StatsPlayer.");
            return;
        }

        StatsPlayer.OnPlayerHealthChange += UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        StatsPlayer.OnPlayerHealthChange -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (statsPlayer) { UpdateUI(statsPlayer.health); }
    }

    public void UpdateUI(Types.HealthState[] health)
    {
        for (int i  = 0; i < health.Length; i++)
        {
            if (!images[i].enabled) { images[i].enabled = true; }
            switch (health[i])
            {
                case Types.HealthState.FILLED:
                    images[i].sprite = spriteFilled;
                    break;
                case Types.HealthState.EMPTY:
                    images[i].sprite = spriteEmpty;
                    break;
            }
        }
    }
}
