using UnityEngine;

public class WidgetTriggerBox : TriggerBox
{
    [SerializeField] private GameObject widgetObject;
    
    protected override void TriggerBehavior()
    {
        if (!widgetObject)
            return;

        GameObject pushedWidget = WidgetManager.PushWidget(widgetObject);
        widgetObject = pushedWidget;
    }
}
