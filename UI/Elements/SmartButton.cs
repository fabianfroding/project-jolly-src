using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmartButton : Button
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        //Debug.Log($"{name} selected");
    }
}
