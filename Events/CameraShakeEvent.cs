public class CameraShakeEvent
{
    public float Duration { get; private set; }
    public float Magnitude { get; private set; }

    public CameraShakeEvent(float duration, float magnitude)
    {
        Duration = duration;
        Magnitude = magnitude;
    }
}
