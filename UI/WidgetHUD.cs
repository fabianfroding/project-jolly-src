using UnityEngine;

public class WidgetHUD : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private GameObject daytimeIndicator;

    private static WidgetHUD instance;
    public static WidgetHUD Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<WidgetHUD>();
            return instance;
        }
    }

    public void ShowInteractionPanel(bool show)
    {
        if (!interactionPanel) { return; }
        interactionPanel.SetActive(show);
        daytimeIndicator.SetActive(!show);
    }

    public void SetInteractionPanelText(string text)
    {
        if (!interactionPanel) { return; }

        WidgetInteractionPanel widgetInteractionPanel = interactionPanel.GetComponent<WidgetInteractionPanel>();
        if (!widgetInteractionPanel) { return; }

        widgetInteractionPanel.SetText(text);
    }
}
