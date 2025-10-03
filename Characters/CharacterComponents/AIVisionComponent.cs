using UnityEngine;

public class AIVisionComponent : CoreComponent
{
    public PlayerCharacter TargetPlayerCharacter { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter triggeringPlayerCharacter = collision.GetComponent<PlayerCharacter>();
        if (triggeringPlayerCharacter)
            TargetPlayerCharacter = triggeringPlayerCharacter;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCharacter triggeringPlayerCharacter = collision.GetComponent<PlayerCharacter>();
        if (triggeringPlayerCharacter)
            TargetPlayerCharacter = null;
    }

    public bool HasTarget()
    {
        if (TargetPlayerCharacter)
            return TargetPlayerCharacter.IsAlive();
        return false;
    }

    public void ResetTarget() => TargetPlayerCharacter = null;
}
