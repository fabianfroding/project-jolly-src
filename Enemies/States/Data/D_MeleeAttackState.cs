using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/StateData/MeleeAttackState")]
public class D_MeleeAttackState : ScriptableObject
{
    public Types.DamageData damageData;

    public GameObject slashHorizontalPrefab;
    [Tooltip("How far the slash should spawn horizontally from the spawn position.")]
    public float spawnPosOffsetX = 2f;

    public LayerMask playerLayer;
}
