using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/StateData")]
public class Player_StateData : ScriptableObject
{
    public LayerMask groundLayer;
    public float defaultGravityScale = 4f;

    [Header("Dead State State")]
    public float deadStateDuration = 3f;

    [Header("Jump State")]
    public float jumpVelocity = 25f;
    public GameObject jumpVFXPrefab;
    public GameObject jumpTrailVFXPrefab;
    public AudioClip jumpAudioClip;
    public AudioClip jumpLandAudioClip;
    public AudioClip doubleJumpAudioClip;

    [Header("Double Jump State")]
    public float doubleJumpVelocity = 20f;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.25f;
    public float maxFallVelocity = -1f;

    [Header("Respawn State")]
    public float respawnDelay = 2f;

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
    public Types.DamageData attackDamageData;
    public GameObject vfxPrefabHorizontal;
    public GameObject vfxPrefabVertical;
    public GameObject attackSlashPrefabHorizontal;
    public GameObject attackSlashPrefabVertical;

    [Header("Take Damage State")]
    public float takeDamageDuration = 1.5f;
    [Range(0f, 1f)]
    public float takeDamageTimeScale = 0.5f;

    [Header("Dash State")]
    public float dashCooldown = 0.7f;
    public float dashDelay = 0.05f;
    public float dashDistance = 12f;
    public GameObject dashVFXPrefab;
    public AudioClip dashAudioClip;

    public float distBetweenAfterimages = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public AudioClip dashTimeSlowAudioClip;
    public float dashVelocity;
    public float dashTime;

    [Header("Warp State")]
    public float ascendRayDistance = 20f;
    public float ascendDiveInRayDistance = 5f;
    public float warpInAirSpeed = 0.14f;
    public float warpInGroundSpeed = 0.07f;
    public GameObject ascendDiveInVFX;
    public GameObject ascendEmergeVFX;
    public AudioClip ascendDiveInAudioClip;
    public AudioClip ascendEmergeAudioClip;
    public GameObject warpActiveSFX;
    public GameObject warpCeilingCheck;

    [Header("Thunder State")]
    public float thunderHeight = 15f;
    public float thunderDelay = 0.25f;
    public float thunderRadius = 7f;
    public float thunderCooldown = 1f;
    public Types.DamageData thunderDamage;
    public AudioClip[] thunderAudioClips;
    public GameObject thunderVFX;

    [Header("Air Glide State")]
    public float airGlideFallVelocity = -1.5f;
    public AudioClip airGlideStartAudioClip;
    public AudioClip airGlideEndAudioClip;
}
