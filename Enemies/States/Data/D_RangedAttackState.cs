using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/StateData/RangedAttackState")]
public class D_RangedAttackState : ScriptableObject
{
    public GameObject projectile;
    public LayerMask playerLayer;
    public int projectileDamage = 1;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance = 8f;
}
