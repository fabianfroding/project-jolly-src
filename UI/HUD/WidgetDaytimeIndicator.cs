using TMPro;
using UnityEngine;

public class WidgetDaytimeIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hourText;

    private void OnEnable()
    {
        DaytimeManager.OnTimeChange += OnTimeChanged;
    }

    private void OnDisable()
    {
        DaytimeManager.OnTimeChange -= OnTimeChanged;
    }

    private void OnTimeChanged(int hour, int minute)
    {
        if (!hourText)
        {
            Debug.LogWarning("WidgetDaytimeIndicator::OnHourChanged: Could not find TextMeshProUGUI component for variable 'hourText'.");
            return;
        }
        hourText.text = "Hour: " + hour;
    }
}
