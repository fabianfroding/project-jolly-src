using TMPro;
using UnityEngine;

public class WidgetDaytimeIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hourText;

    private void OnEnable()
    {
        DaytimeManager.OnHourChange += OnHourChanged;
    }

    private void OnDisable()
    {
        DaytimeManager.OnHourChange -= OnHourChanged;
    }

    private void OnHourChanged(int hour)
    {
        if (!hourText)
        {
            Debug.LogWarning("WidgetDaytimeIndicator::OnHourChanged: Could not find TextMeshProUGUI component for variable 'hourText'.");
            return;
        }
        hourText.text = "Hour: " + hour;
    }
}
