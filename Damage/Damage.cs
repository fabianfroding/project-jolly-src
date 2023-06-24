using UnityEngine;

[CreateAssetMenu(fileName = "newDamage", menuName = "Data/DamageData/Damage")]
public class Damage : ScriptableObject
{
    public int damageAmount;
    public Vector2 direction;
    public float stunDamageAmount;
    public float knockbackStrength;
    public Vector2 knockbackAngle;

    public DAMAGE_TYPE damageType;
    public DAMAGE_RANGE damageRange;

    public enum DAMAGE_TYPE
    {
        ENVIRONMENT,
        PHYSICAL,
        MAGICAL
    }

    public enum DAMAGE_RANGE
    {
        MELEE,
        RANGED
    }
}
