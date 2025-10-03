using UnityEngine;

public class HealthCombatEvent
{
    private readonly int newHealth;
    private readonly GameObject damagedObject;

    public HealthCombatEvent(int newHealth,  GameObject damagedObject)
    {
        this.newHealth = newHealth;
        this.damagedObject = damagedObject;
    }
    
    public int GetNewHealth() => newHealth;
    public GameObject GetDamagedObject() => damagedObject;
}
