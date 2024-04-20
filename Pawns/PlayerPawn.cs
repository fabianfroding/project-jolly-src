using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerPawn : PawnBase
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDoubleJumpState DoubleJumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerChargeArrowState ChargeArrowState { get; set; }
    public PlayerFireArrowState FireArrowState { get; set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerTakeDamageState TakeDamageState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    //public PlayerDyingState DyingState { get; private set; }
    public PlayerPickupPowerupState PickupPowerupState { get; private set; }
    public PlayerLockedState LockedState { get; private set; }
    public PlayerHoldAscendState HoldAscendState { get; private set; }
    public PlayerAscendState AscendState { get; private set; }
    public PlayerThunderState ThunderState { get; private set; }
    public PlayerAirGlideState AirGlideState { get; private set; }
    public PlayerFloatingBubbleState FloatingBubbleState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    #endregion

    #region Other Variables
    [SerializeField] private Player_StateData playerStateData;

    [Range(0, 100)]
    [SerializeField] private float playerDaytimeVisibilityRadius = 0f;
    [Range(0, 100)]
    [SerializeField] private float playerNighttimeVisibilityRadius = 0f;
    [Range(0, 100)]
    [SerializeField] private float playerDawnAndDuskVisibility = 0f;
    [SerializeField] private Color playerDaytimeLightColor = Color.white;
    [SerializeField] private Color playerNighttimeLightColor = Color.blue;
    [SerializeField] private Color playerDawnAndDuskLightColor = Color.yellow;

    public GameObject attackHorizontalDamageHitBox;
    public GameObject attackUpDamageHitBox;
    public GameObject attackDownDamageHitBox;
    [SerializeField] private Transform fireArrowSpawnTransform;
    [SerializeField] private Types.DamageData enemyCollisionDamage;

    public Light2D light2D;
    public IInteractable currentInteractionTarget;

    private float cachedDawnMid;
    private float cachedDuskMid;
    #endregion

    #region Events
    public static event Action<PlayerPawn> OnPlayerAwake;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerRevive;
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
        ChargeArrowState = new PlayerChargeArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_CHARGE_ARROW);
        FireArrowState = new PlayerFireArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_FIRE_ARROW);
        AttackState = new PlayerAttackState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ATTACK);
        TakeDamageState = new PlayerTakeDamageState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        DeadState = new PlayerDeadState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DEAD);
        //DyingState = new PlayerDyingState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DYING);
        PickupPowerupState = new PlayerPickupPowerupState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_PICKUP_POWERUP);
        LockedState = new PlayerLockedState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);
        FloatingBubbleState = new PlayerFloatingBubbleState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        InteractState = new PlayerInteractState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);

        InputHandler = GetComponent<PlayerInputHandler>();

        DaytimeManager.OnDaytimeTick += UpdateDaytimeVisibility;

        OnPlayerAwake?.Invoke(this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);

        cachedDawnMid = DaytimeManager.Instance.GetDawnMidTime();
        cachedDuskMid = DaytimeManager.Instance.GetDuskMidTime();

        if (HealthComponent)
            HealthComponent.OnDamageTaken += OnDamageTaken;
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
        EnemyPawn enemy = collision.gameObject.GetComponent<EnemyPawn>();
        if (enemy)
        {
            enemyCollisionDamage.source = collision.gameObject;
            enemyCollisionDamage.target = gameObject;
            HealthComponent.TakeDamage(enemyCollisionDamage);
            Movement.Knockback(enemyCollisionDamage.knockbackAngle, enemyCollisionDamage.knockbackStrength, Movement.FacingDirection);
        }
    }

    private void OnDestroy()
    {
        DaytimeManager.OnDaytimeTick -= UpdateDaytimeVisibility;
        if (HealthComponent)
            HealthComponent.OnDamageTaken -= OnDamageTaken;
    }
    #endregion

    #region Enable State Functions
    public void EnableAllLockedStates()
    {
        EnableDoubleJumpState();
        EnableWallJumpAndSlideStates();
        EnableDashState();
        EnableAirGlideState();
        EnableWarpState();
        EnableThunderState();
    }

    public void EnableDoubleJumpState() => DoubleJumpState = new PlayerDoubleJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);

    public void EnableWallJumpAndSlideStates()
    {
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_WALL_SLIDE);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
    }

    public void EnableDashState()
    {
        DashState = new PlayerDashState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
    }

    public void EnableAirGlideState()
    {
        AirGlideState = new PlayerAirGlideState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_AIR_GLIDE);
    }

    public void EnableWarpState()
    {
        HoldAscendState = new PlayerHoldAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_HOLD_ASCEND);
        AscendState = new PlayerAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ASCEND);
    }

    public void EnableThunderState()
    {
        ThunderState = new PlayerThunderState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_THUNDER);
    }
    #endregion

    #region Other Functions
    private void OnDamageTaken(Types.DamageData damageData)
    {
        if (HealthComponent.IsAlive() && StateMachine.CurrentState != TakeDamageState && !HealthComponent.IsInvulnerable())
        {
            HealthComponent.SetInvulnerable(true);

            // Check so that player is not dead to avoid respawning when reviving.
            if (HealthComponent.IsAlive())
            {
                if (!damageData.source.GetComponent<PawnBase>())
                {
                    OnPlayerTakeDamageFromENV?.Invoke();
                }
                else
                {
                    Vector2 dir = GameFunctionLibrary.GetDirectionFromAngle(GameFunctionLibrary.GetAngleBetweenObjects(damageData.source, gameObject));
                    Movement.Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
                    StateMachine.ChangeState(TakeDamageState);
                }
            }
        }
        else if (!HealthComponent.IsAlive())
        {
            StateMachine.ChangeState(DeadState);
        }
    }

    protected override void Death()
    {
        base.Death();
        Debug.Log("Death");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
    }

    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public override void Revive()
    {
        base.Revive();
        OnPlayerRevive?.Invoke();
    }

    public void RevivePlayer()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY), false);
        HealthComponent.SetHealth(HealthComponent.GetMaxHealth());
        ResetState();
    }

    public bool IsDead() => StateMachine.CurrentState == DeadState;

    public bool Interact()
    {
        if (currentInteractionTarget != null)
        {
            currentInteractionTarget.Interact();
            DaytimeManager.Instance.stopDaytime = true;
            return true;
        }
        return false;
    }

    public bool AdvanceInteraction()
    {
        if (currentInteractionTarget == null)
            return false;
        if (currentInteractionTarget.AdvanceInteraction())
            return true;
        return false;
    }

    public void EndInteraction()
    {
        WidgetHUD widgetHUD = WidgetHUD.Instance;
        if (widgetHUD != null)
            WidgetHUD.Instance.ShowInteractionPanel(false);
        DaytimeManager.Instance.stopDaytime = false;
    }

    public void UpdateDaytimeVisibility(float timeOfDay)
    {
        if (light2D)
        {
            float dawnStart = DaytimeManager.Instance.GetDawnStartTime();
            float dawnEnd = DaytimeManager.Instance.GetDawnEndTime();
            float duskStart = DaytimeManager.Instance.GetDuskStartTime();
            float duskEnd = DaytimeManager.Instance.GetDuskEndTime();

            if (timeOfDay >= dawnStart && timeOfDay < cachedDawnMid)
            {
                float lerpFactor = Mathf.Clamp01((timeOfDay - dawnStart) / (cachedDawnMid - dawnStart));
                light2D.pointLightOuterRadius = Mathf.Lerp(playerNighttimeVisibilityRadius, playerDawnAndDuskVisibility, lerpFactor);
                light2D.color = Color.Lerp(playerNighttimeLightColor, playerDawnAndDuskLightColor, lerpFactor);
            }
            else if (timeOfDay >= cachedDawnMid && timeOfDay < dawnEnd)
            {
                float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDawnMid) / (dawnEnd - cachedDawnMid));
                light2D.pointLightOuterRadius = Mathf.Lerp(playerDawnAndDuskVisibility, playerDaytimeVisibilityRadius, lerpFactor);
                light2D.color = Color.Lerp(playerDawnAndDuskLightColor, playerDaytimeLightColor, lerpFactor);
            }
            else if (timeOfDay >= duskStart && timeOfDay < cachedDuskMid)
            {
                float lerpFactor = Mathf.Clamp01((timeOfDay - duskStart) / (cachedDuskMid - duskStart));
                light2D.pointLightOuterRadius = Mathf.Lerp(playerDaytimeVisibilityRadius, playerDawnAndDuskVisibility, lerpFactor);
                light2D.color = Color.Lerp(playerDaytimeLightColor, playerDawnAndDuskLightColor, lerpFactor);
            }
            else if (timeOfDay >= cachedDuskMid && timeOfDay < duskEnd)
            {
                float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDuskMid) / (duskEnd - cachedDuskMid));
                light2D.pointLightOuterRadius = Mathf.Lerp(playerDawnAndDuskVisibility, playerNighttimeVisibilityRadius, lerpFactor);
                light2D.color = Color.Lerp(playerDawnAndDuskLightColor, playerNighttimeLightColor, lerpFactor);
            }
            else
            {
                light2D.pointLightOuterRadius = (timeOfDay < dawnStart || timeOfDay >= duskEnd) ? playerNighttimeVisibilityRadius : playerDaytimeVisibilityRadius;
                light2D.color = (timeOfDay < dawnStart || timeOfDay >= duskEnd) ? playerNighttimeLightColor : playerDaytimeLightColor;
            }
        }
    }

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

    public bool HasXMovementInput() =>  InputHandler.NormInputX != 0;

    public Vector2 GetFireArrowSpawnPosition() => fireArrowSpawnTransform.position;

    public void TriggerOnPlayerEnterAirGlideState() => OnPlayerEnterAirGlideState();
    public void TriggerOnPlayerExitAirGlideState() => OnPlayerExitAirGlideState();

    public Player_StateData GetPlayerStateData() => playerStateData;

    public int GetFacingDirection() => Movement.FacingDirection;

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
