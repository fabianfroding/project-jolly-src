using System;
using TMPro;

public class WidgetConfirmPrompt : WidgetBase
{
    private TextMeshProUGUI textMeshProUGUI;
    
    public event Action<bool> OnConfirm;

    protected override void Awake()
    {
        base.Awake();
        
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Confirm(bool confirmed)
    {
        OnConfirm?.Invoke(confirmed);
        Pop();
    }
    
    public void SetTitle(string title)
    {
        textMeshProUGUI.text = title;
    }
}
