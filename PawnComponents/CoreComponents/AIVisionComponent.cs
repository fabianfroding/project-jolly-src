using UnityEngine;

public class AIVisionComponent : CoreComponent
{
    public PlayerPawn TargetPlayerPawn { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPawn triggeringPlayerPawn = collision.GetComponent<PlayerPawn>();
        if (triggeringPlayerPawn)
            TargetPlayerPawn = triggeringPlayerPawn;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerPawn triggeringPlayerPawn = collision.GetComponent<PlayerPawn>();
        if (triggeringPlayerPawn)
            TargetPlayerPawn = null;
    }

    public bool HasTarget()
    {
        if (TargetPlayerPawn)
            return TargetPlayerPawn.IsAlive();
        return false;
    }

    public void ResetTarget() => TargetPlayerPawn = null;
}
