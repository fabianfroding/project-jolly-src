using UnityEngine;

[System.Serializable]
public class CameraShakeEvent
{
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;

    public CameraShakeEvent(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
    }

    public float GetDuration() => duration;
    public float GetMagnitude() => magnitude;
}
