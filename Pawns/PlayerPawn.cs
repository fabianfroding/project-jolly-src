using UnityEngine;

public class PlayerPawn : PawnBase
{
    #region State Variables
    [SerializeField] private Player_StateData playerStateData;
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerTakeDamageState TakeDamageState { get; private set; }
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerStateData, AnimationConstants.ANIM_PARAM_IDLE);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }
    #endregion
}
