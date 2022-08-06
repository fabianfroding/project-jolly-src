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

    public enum DAMAGE_TYPE
    {
        ENVIRONMENT,
        PHYSICAL,
        MAGICAL
    }
}
