using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : UIIngameWidget
{
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite spriteFilled; 
    [SerializeField] private Sprite spriteEmpty;
    [SerializeField] private Sprite spriteBroken;
    private StatsPlayer statsPlayer;

    private void Awake()
    {
        statsPlayer = FindObjectOfType<StatsPlayer>();
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
        if (!statsPlayer) 
        {
            statsPlayer = FindObjectOfType<StatsPlayer>();
        }
        UpdateUI(statsPlayer.CurrentHealth, statsPlayer.GetMaxHealth(), statsPlayer.BrokenHealth);
    }

    public void UpdateUI(int currentHealth, int maxHealth, int brokenHealth)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = i <= maxHealth;
            images[i].sprite = i <= currentHealth ? spriteFilled : spriteEmpty;
        }

        for (int i = maxHealth; brokenHealth > 0; i--)
        {

        }
    }
}
