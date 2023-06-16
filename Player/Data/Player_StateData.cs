using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/StateData")]
public class Player_StateData : ScriptableObject
{
    public LayerMask groundLayer;
    public float defaultGravityScale = 4f;

    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 25f;
    public int amountOfJumps = 1;
    public GameObject jumpSFXPrefab;
    public GameObject jumpTrailSFXPrefab;
    public GameObject jumpSNDPrefab;
    public GameObject landSoundPrefab;
    public GameObject doubleJumpSFX;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.25f;

    [Header("Wall Touch State")]
    public float wallFrictionMultiplier = 0.99f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = -3f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1f, 2f);

    [Header("Arrow States")]
    public float chargeArrowCooldown = 0.5f;
    public float arrowUpwardVelocity = 15f;
    public float arrowDownwardVelocity = 7.5f;
    public float arrowSidewayVelocity = 15f;
    public GameObject arrowPrefab;

    [Header("Attack State")]
    public GameObject attackSlashPrefabHorizontal;
    public GameObject attackSlashPrefabVertical;

    [Header("Dash State")]
    public float dashCooldown = 0.7f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;

    public float distBetweenAfterimages = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public GameObject dashSFX;
    public GameObject dashTimeSlowSFX;

    [Header("Warp State")]
    public float ascendRayDistance = 20f;
    public float ascendDiveInRayDistance = 5f;
    public GameObject ascendDiveInVFX;
    public GameObject ascendEmergeVFX;
    public GameObject ascendDiveInSFX;
    public GameObject ascendEmergeSFX;
    public GameObject warpCeilingCheck;

    [Header("Thunder State")]
    public float thunderHeight = 15f;
    public float thunderDelay = 0.25f;
    public float thunderRadius = 7f;
    public float thunderCooldown = 1f;
    public Damage thunderDamage;
    public GameObject thunderSFX;
    public GameObject thunderVFX;

    [Header("Air Glide State")]
    public float airGlideFallVelocity = -1.5f;
    public GameObject airGlideStartSFX;
    public GameObject airGlideEndSFX;
}
