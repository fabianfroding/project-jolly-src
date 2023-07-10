using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/StateData/MeleeAttackState")]
public class D_MeleeAttackState : ScriptableObject
{
    public float attackRadius = 0.5f;
    public Types.DamageData damageData;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 5f;

    public GameObject slashHorizontalPrefab;
    [Tooltip("How far the slash should spawn horizontally from the spawn position.")]
    public float spawnPosOffsetX = 2f;

    public LayerMask playerLayer;
}
