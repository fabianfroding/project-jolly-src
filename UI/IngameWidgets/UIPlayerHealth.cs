using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : UIIngameWidget
{
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite spriteFilled; 
    [SerializeField] private Sprite spriteEmpty;

    private void Awake()
    {
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
        StatsPlayer statsPlayer = FindObjectOfType<StatsPlayer>();
        if (statsPlayer != null)
        {
            // TODO: Duct tape. Needs to ensure we actually get the stats.
            UpdateUI(statsPlayer.currentHealth, statsPlayer.GetMaxHealth());
        }
    }

    public void UpdateUI(int health, int maxHealth)
    {
        if (health > maxHealth) health = maxHealth;

        for (int i = 0; i < images.Length; i++)
        {
            if (i < health) images[i].sprite = spriteFilled;
            else images[i].sprite = spriteEmpty;

            if (i < maxHealth) images[i].enabled = true;
            else images[i].enabled = false;
        }
    }
}
