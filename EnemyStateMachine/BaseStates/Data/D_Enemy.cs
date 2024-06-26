using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/EnemyData/BaseData")]
public class D_Enemy : ScriptableObject
{
    public bool isFlying = false;

    [Tooltip("Select to choose when the enemy should respawn.")]
    public EnemyRespawnType enemyRespawnType = EnemyRespawnType.ON_SAVE;

    public float damageHopSpeed = 3f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    public float maxAggroDistance = 4f;
    public float minAggroDistance = 3f;

    public float closeRangeActionDistance = 1f;
    public float longRangeActionDistance = 5f;

    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public enum EnemyRespawnType
    {
        ON_SAVE,
        ON_SCENE_TRANSITION,
        NEVER,
    }
}
