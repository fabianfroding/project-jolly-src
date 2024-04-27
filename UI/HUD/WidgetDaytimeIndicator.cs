using TMPro;
using UnityEngine;

public class WidgetDaytimeIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] SODaytimeSettings daytimeSettings;

    public void OnTimeChanged()
    {
        if (!hourText)
        {
            Debug.LogWarning("WidgetDaytimeIndicator::OnHourChanged: Could not find TextMeshProUGUI component for variable 'hourText'.");
            return;
        }
        hourText.text = "Hour: " + daytimeSettings.currentHour;
    }
}
