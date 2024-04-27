using TMPro;
using UnityEngine;

public class WidgetDaytimeIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hourText;
    [SerializeField] SOIntVariable currentHour;
    [SerializeField] SOIntVariable currentMinute;

    public void OnTimeChanged()
    {
        if (!hourText)
        {
            Debug.LogWarning("WidgetDaytimeIndicator::OnHourChanged: Could not find TextMeshProUGUI component for variable 'hourText'.");
            return;
        }
        hourText.text = "Hour: " + currentHour.Value;
    }
}
