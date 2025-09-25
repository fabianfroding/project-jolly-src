using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayerController : PlayerController, ILogicUpdate
{
    [SerializeField] private string GameplayActionMap;
    [SerializeField] private EInputMode initialInputMode = EInputMode.Gameplay;
    
    [Header("Gameplay Input Actions")]
    [SerializeField] private InputActionReference IA_Jump;
    [SerializeField] private InputActionReference IA_Move;
    [SerializeField] private InputActionReference IA_Pause;
    
    [Header("Gameplay Input Settings")]
    [SerializeField] private float jumpInputHoldTime = 0.2f;
    
    [SerializeField] private GameObject pauseMenuWidget;
    
    public Vector2 RawMovementInput { get; private set; }
    
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    private float jumpInputStartTime;

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
    }
    
    public void UseJumpInput() => JumpInput = false;
    
    protected override void HandleInput(InputAction.CallbackContext context)
    {
        base.HandleInput(context);
        
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
        
        WidgetManager.PushWidget(pauseMenuWidget);
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
