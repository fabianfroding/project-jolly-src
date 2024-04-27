using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public SOGameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response?.Invoke();
    }
}
