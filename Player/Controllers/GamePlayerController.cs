using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayerController : PlayerController, ILogicUpdate
{
    [SerializeField] private string GameplayActionMap;
    [SerializeField] private EInputMode initialInputMode = EInputMode.Gameplay;

    [Header("Gameplay Input Actions")]
    [SerializeField] private InputActionReference IA_AdvanceInteraction;
    [SerializeField] private InputActionReference IA_AirGlide;
    [SerializeField] private InputActionReference IA_Attack;
    [SerializeField] private InputActionReference IA_ChargeBow;
    [SerializeField] private InputActionReference IA_Dash;
    [SerializeField] private InputActionReference IA_Interact;
    [SerializeField] private InputActionReference IA_Jump;
    [SerializeField] private InputActionReference IA_Move;
    [SerializeField] private InputActionReference IA_Pause;
    [SerializeField] private InputActionReference IA_Warp;
    [SerializeField] private InputActionReference IA_WarpDirection;
    
    [Header("Gameplay Input Settings")]
    [SerializeField] private float jumpInputHoldTime = 0.2f;
    [SerializeField] private float dashInputHoldTime = 0.2f;
    
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawWarpDirectionInput { get; private set; }
    public Vector2Int WarpDirectionInput { get; private set; }
    
    public bool AdvanceInteractionInput {  get; private set; }
    public bool AirGlideInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool ChargeBowInput { get; private set; }
    public bool ChargeBowInputRelease { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool HoldWarpInput { get; private set; }
    public bool HoldWarpInputStop { get; private set; }
    public bool InteractInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    private float jumpInputStartTime;
    private float dashInputStartTime;

    protected override void Awake()
    {
        base.Awake();
        SetInputMode(initialInputMode);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<InputModeEvent>(HandleInputModeChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<InputModeEvent>(HandleInputModeChanged);
    }
    
    void ILogicUpdate.LogicUpdate()
    {
        if (Time.time >= jumpInputStartTime + jumpInputHoldTime)
            JumpInput = false;
        
        if (Time.time >= dashInputStartTime + dashInputHoldTime)
            DashInput = false;
    }
    
    public void UseAdvanceInteractionInput() => AdvanceInteractionInput = false;
    public void UseAttackInput() => AttackInput = false;
    public void UseChargeBowInput() => ChargeBowInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseHoldWarpInput() => HoldWarpInput = false;
    public void UseInteractInput() => InteractInput = false;
    public void UseJumpInput() => JumpInput = false;
    
    protected override void HandleInput(InputAction.CallbackContext context)
    {
        base.HandleInput(context);

        if (context.action == IA_AirGlide.action)
        {
            HandleAirGlideInput(context);
        }

        if (context.action == IA_Attack.action)
        {
            HandleAttackInput(context);
        }

        if (context.action == IA_ChargeBow.action)
        {
            HandleChargeBowInput(context);
        }

        if (context.action == IA_Dash.action)
        {
            HandleDashInput(context);
        }
        
        if (context.action == IA_Jump.action)
        {
            HandleJumpInput(context);
            jumpInputStartTime = Time.time;
        }
        
        if (context.action == IA_Move.action)
        {
            HandleMoveInput(context);
        }

        if (context.action == IA_Pause.action)
        {
            HandlePauseInput(context);
        }

        if (context.action == IA_Warp.action)
        {
            HandleWarpInput(context);
        }
        
        if (context.action == IA_WarpDirection.action)
        {
            HandleWarpDirectionInput(context);
        }
    }

    private void HandleAirGlideInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AirGlideInput = true;
        }
        if (context.canceled)
        {
            AirGlideInput = false;
        }
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }

        if (context.canceled)
        {
            AttackInput = false;
        }
    }
    
    private void HandleChargeBowInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChargeBowInput = true;
            ChargeBowInputRelease = false;
        }
        else if (context.canceled)
        {
            ChargeBowInput = false;
            ChargeBowInputRelease = true;
        }
    }

    private void HandleDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInput = false;
            DashInputStop = true;
        }
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInput = false;
            JumpInputStop = true;
        }
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(RawMovementInput.x); // > 0.5 => 1, < 0.5 = 0.
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    private void HandlePauseInput(InputAction.CallbackContext context)
    {
        if (!context.started) 
            return;
        
        InputActionEvent pushPauseMenuEvent = new(context.action);
        EventBus.Publish(pushPauseMenuEvent);
    }
    
    public void HandleWarpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HoldWarpInput = true;
            HoldWarpInputStop = false;
        }
        else if (context.canceled)
        {
            HoldWarpInput = false;
            HoldWarpInputStop = true;
        }
    }

    public void HandleWarpDirectionInput(InputAction.CallbackContext context)
    {
        RawWarpDirectionInput = context.ReadValue<Vector2>();
        WarpDirectionInput = Vector2Int.RoundToInt(RawWarpDirectionInput.normalized);
    }
    
    private void HandleInputModeChanged(InputModeEvent inputModeEvent)
    {
        SetInputMode(inputModeEvent.GetNewInputMode());
    }

    private void SetInputMode(EInputMode newInputMode)
    {
        currentInputMode = newInputMode;
        switch (currentInputMode)
        {
            case EInputMode.Gameplay:
            {
                playerInput.SwitchCurrentActionMap(GameplayActionMap);
                break;
            }
            case EInputMode.UI:
            {
                playerInput.SwitchCurrentActionMap(UIActionMap);
                break;
            }
        }
    }
}
