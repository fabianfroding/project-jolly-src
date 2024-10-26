using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerPawn : PawnBase
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerDoubleJumpState DoubleJumpState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRespawnState RespawnState { get; private set; }

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
    public PlayerHoldAscendState HoldAscendState { get; private set; }
    public PlayerAscendState AscendState { get; private set; }
    public PlayerAirGlideState AirGlideState { get; private set; }
    public PlayerFloatingBubbleState FloatingBubbleState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    #endregion

    #region Alt State Variables
    public PlayerAttackStateAlt AttackStateAlt { get; private set; }
    public PlayerIdleStateAlt IdleStateAlt { get; private set; }
    public PlayerInAirStateAlt InAirStateAlt { get; private set; }
    public PlayerJumpStateAlt JumpStateAlt { get; private set; }
    public PlayerLandStateAlt LandStateAlt { get; private set; }
    public PlayerMoveStateAlt MoveStateAlt { get; private set; }
    #endregion

    public PlayerInputHandler InputHandler { get; private set; }

    #region Other Variables
    [SerializeField] private Player_StateData playerStateData;
    [SerializeField] private SOGameEvent OnPlayerHealthChangedGameEvent;
    [SerializeField] private SOGameEvent OnPlayerMaxHealthChangedGameEvent;
    [SerializeField] private SOGameEvent SOPlayerInteractEvent;
    [SerializeField] private SOGameEvent SOPlayerInteractionAdvancedEvent;
    [SerializeField] private SOGameEvent SOPlayerInteractEndEvent;
    [SerializeField] private SOInteractionData SOInteractionData;

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

    public GameObject attackHorizontalDamageHitBoxAlt1;

    [SerializeField] private Transform fireArrowSpawnTransform;
    [SerializeField] private Types.DamageData enemyCollisionDamage;

    [SerializeField] private SODaytimeSettings daytimeSettings;

    public Light2D light2D;
    private PlayerInvulnerabilityIndicator playerInvulnerabilityIndicator;
    public string currentSceneName;

    public BoxCollider2D boxCollider;
    public GameObject dashDamageCollider;
    #endregion

    #region Events
    public static event Action<PlayerPawn> OnPlayerAwake;
    public static event Action<PlayerPawn> OnPlayerDeathSequenceFinish;
    public static event Action OnQuitInput;

    public static event Action OnPlayerEnterAirGlideState;
    public static event Action OnPlayerExitAirGlideState;
    #endregion

    public static PlayerPawn Instance { get; private set; }

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_MOVE);
        RespawnState = new PlayerRespawnState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DEAD);
        JumpState = new PlayerJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        InAirState = new PlayerInAirState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        LandState = new PlayerLandState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_LAND);
        ChargeArrowState = new PlayerChargeArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_CHARGE_ARROW);
        FireArrowState = new PlayerFireArrowState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_FIRE_ARROW);
        AttackState = new PlayerAttackState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ATTACK);
        TakeDamageState = new PlayerTakeDamageState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        DeadState = new PlayerDeadState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DEAD);
        FloatingBubbleState = new PlayerFloatingBubbleState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
        InteractState = new PlayerInteractState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);

        AttackStateAlt = new PlayerAttackStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ATTACK_ALT);
        IdleStateAlt = new PlayerIdleStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE_ALT);
        InAirStateAlt = new PlayerInAirStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR_ALT);
        JumpStateAlt = new PlayerJumpStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_JUMP_ALT);
        LandStateAlt = new PlayerLandStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_LAND_ALT);
        MoveStateAlt = new PlayerMoveStateAlt(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_MOVE_ALT);

        EnableUnlockedPlayerAbilities();

        InputHandler = GetComponent<PlayerInputHandler>();
        playerInvulnerabilityIndicator = GetComponent<PlayerInvulnerabilityIndicator>();

        OnPlayerAwake?.Invoke(this);
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);

        if (SaveManager.DoesPlayerSaveDataExist())
        {
            PlayerSaveData playerSaveData = SaveManager.LoadPlayerSaveData();
            HealthComponent.SetMaxHealth(playerSaveData.playerMaxHealth);
            HealthComponent.SetHealth(playerSaveData.playerHealth);
            
            // TODO: Need to rework this. If player re-enters save scene he will get teleported to save position...
            if (SceneManager.GetActiveScene().name == playerSaveData.sceneName)
                transform.position = new Vector2(playerSaveData.position[0], playerSaveData.position[1]);
        }

        SetPlayerRespawnPosition(transform.position);
        currentSceneName = SceneManager.GetActiveScene().name;

        if (OnPlayerMaxHealthChangedGameEvent)
            OnPlayerMaxHealthChangedGameEvent.Raise();
        if (OnPlayerHealthChangedGameEvent)
            OnPlayerHealthChangedGameEvent.Raise();
    }

    protected override void Update()
    {
        base.Update();
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        if (InputHandler.QuitInput)
            OnQuitInput?.Invoke();
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
            TakeDamage(enemyCollisionDamage);
        }
    }

    private void OnEnable()
    {
        PlayerManaComponent.OnPlayerManaFilled += OnPlayerManaFilled;
    }

    private void OnDisable()
    {
        PlayerManaComponent.OnPlayerManaFilled -= OnPlayerManaFilled;
    }
    #endregion

    #region Enable State Functions
    public void EnableAllLockedStates()
    {
        for (int i = 0; i < (int)Types.EUnlockablePlayerAbilityID.Num; i++)
        {
            EnableUnlockablePlayerAbility((Types.EUnlockablePlayerAbilityID)i);
        }
    }

    public void EnableUnlockablePlayerAbility(Types.EUnlockablePlayerAbilityID unlockablePlayerAbilityID)
    {
        switch (unlockablePlayerAbilityID)
        {
            case Types.EUnlockablePlayerAbilityID.Dash:
                DashState = new PlayerDashState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_DASH);
                break;
            case Types.EUnlockablePlayerAbilityID.DoubleJump:
                DoubleJumpState = new PlayerDoubleJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
                break;
            case Types.EUnlockablePlayerAbilityID.WallJump:
                WallSlideState = new PlayerWallSlideState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_WALL_SLIDE);
                WallJumpState = new PlayerWallJumpState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IN_AIR);
                break;
            case Types.EUnlockablePlayerAbilityID.Warp:
                HoldAscendState = new PlayerHoldAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_HOLD_ASCEND);
                AscendState = new PlayerAscendState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_ASCEND);
                break;
        }
    }
    
    public void EnableUnlockedPlayerAbilities()
    {
        for (int i = 0; i < (int)Types.EUnlockablePlayerAbilityID.Num; i++)
        {
            Types.EUnlockablePlayerAbilityID intEnum = (Types.EUnlockablePlayerAbilityID)i;
            if (SaveManager.IsUnlockablePlayerAbilityUnlocked(intEnum))
                EnableUnlockablePlayerAbility(intEnum);
        }
    }
    #endregion

    #region Other Functions

    private void OnPlayerManaFilled()
    {
        StateMachine.ChangeState(IdleStateAlt);
        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.enterAltStateSound, transform.position);
    }

    public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);

        if (!HealthComponent.IsAlive())
        {
            StateMachine.ChangeState(DeadState);
        }
        else if (HealthComponent.IsAlive() && damageData.isKillZone)
        {
            StateMachine.ChangeState(RespawnState);
        }
        else
        {
            StateMachine.ChangeState(TakeDamageState);
            Vector2 dir = GameFunctionLibrary.GetDirectionFromAngle(GameFunctionLibrary.GetAngleBetweenObjects(damageData.source, gameObject));
            Movement.Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
        }

        if (HealthComponent.IsAlive() && HealthComponent.IsInvulnerable() && playerInvulnerabilityIndicator)
            playerInvulnerabilityIndicator.StartFlash();

        HealthComponent.TakeDamage(damageData);
        if (OnPlayerHealthChangedGameEvent)
            OnPlayerHealthChangedGameEvent.Raise();
    }

    public void OnEnterInteractionIndicator(IInteractable interactable)
    {
        if (interactable != null && interactable.IsInteractable() && SOInteractionData)
            SOInteractionData.interactable = interactable;
    }

    public void OnExitInteractionIndicator()
    {
        if (SOInteractionData)
            SOInteractionData.interactable = null;
    }

    public override void Revive()
    {
        base.Revive();
        HealthComponent.SetHealth(3);
        OnPlayerHealthChangedGameEvent.Raise();
        StateMachine.ChangeState(IdleState);
    }

    public override void Death()
    {
        base.Death();
        Rigidbody2D.isKinematic = true;
        Collider2D.enabled = false;
    }

    public void PlayerDeathSequenceFinish()
    {
        Rigidbody2D.isKinematic = false;
        Collider2D.enabled = true;
        OnPlayerDeathSequenceFinish?.Invoke(this);
    }

    public void SetPlayerRespawnPosition(Vector2 pos) => RespawnState?.SetRespawnPosition(pos);

    public bool IsDead() => StateMachine.CurrentState == DeadState;

    public bool Interact()
    {
        if (SOInteractionData && SOInteractionData.interactable != null && SOInteractionData.interactable.IsInteractable())
        {
            SOInteractionData.interactable.Interact();
            SOPlayerInteractEvent.Raise();
            return true;
        }
        return false;
    }

    public bool AdvanceInteraction()
    {
        if (SOInteractionData && SOInteractionData.interactable != null)
        {
            if (SOInteractionData.interactable.AdvanceInteraction())
            {
                if (SOPlayerInteractionAdvancedEvent)
                    SOPlayerInteractionAdvancedEvent.Raise();
                return true;
            }
        }
        return false;
    }

    public void EndInteraction()
    {
        if (SOPlayerInteractEndEvent)
        {
            if (SOInteractionData && SOInteractionData.interactable != null)
                SOInteractionData.interactable.InteractEnd();
            SOPlayerInteractEndEvent.Raise();
        }
    }

    public void UpdateDaytimeVisibility()
    {
        if (!light2D) return;
        if (!daytimeSettings) return;

        float timeOfDay = daytimeSettings.currentHour + (daytimeSettings.currentMinute / 60f);
        float dawnMidTime = daytimeSettings.DawnMidTime; // TODO: These two can be cached to save performance.
        float duskMidTIme = daytimeSettings.DuskMidTime;

        if (timeOfDay >= daytimeSettings.DawnStartTime && timeOfDay < dawnMidTime)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - daytimeSettings.DawnStartTime) / (dawnMidTime - daytimeSettings.DawnStartTime));
            light2D.pointLightOuterRadius = Mathf.Lerp(playerNighttimeVisibilityRadius, playerDawnAndDuskVisibility, lerpFactor);
            light2D.color = Color.Lerp(playerNighttimeLightColor, playerDawnAndDuskLightColor, lerpFactor);
        }
        else if (timeOfDay >= dawnMidTime && timeOfDay < daytimeSettings.DawnEndTime)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - dawnMidTime) / (daytimeSettings.DawnEndTime - dawnMidTime));
            light2D.pointLightOuterRadius = Mathf.Lerp(playerDawnAndDuskVisibility, playerDaytimeVisibilityRadius, lerpFactor);
            light2D.color = Color.Lerp(playerDawnAndDuskLightColor, playerDaytimeLightColor, lerpFactor);
        }
        else if (timeOfDay >= daytimeSettings.DuskStartTime && timeOfDay < duskMidTIme)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - daytimeSettings.DuskStartTime) / (duskMidTIme - daytimeSettings.DuskStartTime));
            light2D.pointLightOuterRadius = Mathf.Lerp(playerDaytimeVisibilityRadius, playerDawnAndDuskVisibility, lerpFactor);
            light2D.color = Color.Lerp(playerDaytimeLightColor, playerDawnAndDuskLightColor, lerpFactor);
        }
        else if (timeOfDay >= duskMidTIme && timeOfDay < daytimeSettings.DuskEndTime)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - duskMidTIme) / (daytimeSettings.DuskEndTime - duskMidTIme));
            light2D.pointLightOuterRadius = Mathf.Lerp(playerDawnAndDuskVisibility, playerNighttimeVisibilityRadius, lerpFactor);
            light2D.color = Color.Lerp(playerDawnAndDuskLightColor, playerNighttimeLightColor, lerpFactor);
        }
        else
        {
            light2D.pointLightOuterRadius = (timeOfDay < daytimeSettings.DawnStartTime || timeOfDay >= daytimeSettings.DuskEndTime) ? playerNighttimeVisibilityRadius : playerDaytimeVisibilityRadius;
            light2D.color = (timeOfDay < daytimeSettings.DawnStartTime || timeOfDay >= daytimeSettings.DuskEndTime) ? playerNighttimeLightColor : playerDaytimeLightColor;
        }
    }

    public void StopInvulnerabilityFlash()
    {
        if (playerInvulnerabilityIndicator)
            playerInvulnerabilityIndicator.EndFlash();
    }

    public bool InAir() => StateMachine.CurrentState == InAirState;

    public bool HasXMovementInput()
    {
        if (InputHandler.enabled)
            return InputHandler.NormInputX != 0;
        return false;
    }

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

    public void SetIgnoreEnemyLayerCollisoon(bool NewCollision)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY), NewCollision);
    }
    #endregion
}
