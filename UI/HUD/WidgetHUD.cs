using UnityEngine;

public class WidgetHUD : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameObject daytimeIndicator;
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject playerMana;

    public void ShowInteractionPanel(bool show)
    {
        if (!interactionPanel)
            return;
        interactionPanel.SetActive(show);
        daytimeIndicator.SetActive(!show);
        playerHealth.SetActive(!show);
        playerMana.SetActive(!show);
    }
}
