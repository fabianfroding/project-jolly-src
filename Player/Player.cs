using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerChargeArrowState ChargeArrowState { get; set; }
    public PlayerFireArrowState FireArrowState { get; set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerDyingState DyingState { get; private set; }
    public PlayerPickupPowerupState PickupPowerupState { get; private set; }
    public PlayerLockedState LockedState { get; private set; }
    public PlayerHoldAscendState HoldAscendState { get; private set; }
    public PlayerAscendState AscendState { get; private set; }
    public PlayerThunderState ThunderState { get; private set; }
    public PlayerAirGlideState AirGlideState { get; private set; }
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    private Timestop timestop;
    private InvulnerabilityIndication invulnerabilityIndication;
    #endregion

    #region Other Variables
    public bool Invulnerable { get; private set; }
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    [SerializeField] private Player_StateData playerStateData;
    [SerializeField] private Transform fireArrowSpawnTransform;
    [SerializeField] private Damage enemyCollisionDamage;
    #endregion

    #region Events
    public static event Action OnPlayerTakeDamageFromENV;
    public static event Action OnPlayerEnterAirGlideState;
    public static event Action OnPlayerExitAirGlideState;
    #endregion

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_MOVE);
        JumpState = new PlayerJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        InAirState = new PlayerInAirState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        LandState = new PlayerLandState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_LAND);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_WALL_SLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        ChargeArrowState = new PlayerChargeArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_CHARGE_ARROW);
        FireArrowState = new PlayerFireArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_FIRE_ARROW);
        AttackState = new PlayerAttackState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ATTACK);
        DashState = new PlayerDashState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        DeadState = new PlayerDeadState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DEAD);
        DyingState = new PlayerDyingState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DYING);
        PickupPowerupState = new PlayerPickupPowerupState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_PICKUP_POWERUP);
        LockedState = new PlayerLockedState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);
        HoldAscendState = new PlayerHoldAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_HOLD_ASCEND);
        AscendState = new PlayerAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ASCEND);
        ThunderState = new PlayerThunderState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_THUNDER);
        AirGlideState = new PlayerAirGlideState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_AIR_GLIDE);

        InputHandler = GetComponent<PlayerInputHandler>();
        timestop = GetComponent<Timestop>();
        invulnerabilityIndication = GetComponent<InvulnerabilityIndication>();
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(EditorConstants.TAG_ENEMY))
        {
            TakeDamage(collision.gameObject, enemyCollisionDamage);
            Combat.Knockback(enemyCollisionDamage.knockbackAngle, enemyCollisionDamage.knockbackStrength, Movement.FacingDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PICKUP_POWERUP))
        {
            StateMachine.ChangeState(PickupPowerupState);
        }
    }
    #endregion

    #region Other Functions
    public override void TakeDamage(GameObject source, Damage damage, GameObject damagingObject = null)
    {
        if (!Invulnerable)
        {
            base.TakeDamage(source, damage, damagingObject);

            if (Stats.currentHealth > 0)
            {
                Invulnerable = true;

                timestop.StopTime(0.05f, 10, 0.1f);

                // Check so that player is not dead to avoid respawning when reviving.
                if (damage.damageType == Damage.DAMAGE_TYPE.ENVIRONMENT)
                {
                    if (StateMachine.CurrentState != DyingState && StateMachine.CurrentState != DeadState)
                    {
                        OnPlayerTakeDamageFromENV?.Invoke();
                    }
                }
                else
                {
                    if (StateMachine.CurrentState != DyingState && StateMachine.CurrentState != DeadState)
                    {
                        Vector2 dir = TrigonometryUtils.GetDirectionFromAngle(TrigonometryUtils.GetAngleBetweenObjects(source, gameObject));
                        Combat.Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
                    } 
                }

                if (StateMachine.CurrentState != DyingState && StateMachine.CurrentState != DeadState)
                {
                    invulnerabilityIndication.StartFlash();
                }

                StopCoroutine(ResetInvulnerability());
                StartCoroutine(ResetInvulnerability());
            }
        }
    }

    protected override void Death()
    {
        base.Death();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
        StateMachine.ChangeState(DyingState);
    }

    public override void Revive()
    {
        base.Revive();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY), false);
        Stats.IncreaseHealth(Stats.GetMaxHealth());
        ResetState(); // TODO: Remove after dead state changes to idle with health check.
    }

    public bool IsDead() => StateMachine.CurrentState == DeadState;

    public void ChangeStateToInDialogue() => StateMachine.ChangeState(LockedState);

    public void ToggleLockState()
    {
        if (StateMachine.CurrentState == LockedState)
        {
            ResetState();
        }
        else
        {
            StateMachine.ChangeState(LockedState);
        }
    }

    public bool InAir() => StateMachine.CurrentState == InAirState;
    public bool InLockedState() => StateMachine.CurrentState == LockedState;

    public void ResetState() => StateMachine.ChangeState(IdleState);

    private IEnumerator ResetInvulnerability()
    {
        yield return new WaitForSeconds(invulnerabilityDuration);
        invulnerabilityIndication.EndFlash();
        Invulnerable = false;
    }

    public bool HasXMovementInput() =>  InputHandler.NormInputX != 0;

    public Vector2 GetFireArrowSpawnPosition() => fireArrowSpawnTransform.position;

    public void TriggerOnPlayerEnterAirGlideState() => OnPlayerEnterAirGlideState();
    public void TriggerOnPlayerExitAirGlideState() => OnPlayerExitAirGlideState();

    public Player_StateData GetPlayerStateData() => playerStateData;

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void AnimationBlink()
    {
        if (UnityEngine.Random.Range(0, 2) != 1)
        {
            Animator.Play(AnimationConstants.ANIM_PARAM_IDLE, 0, 0.0f);
        }
    }
    #endregion
}
