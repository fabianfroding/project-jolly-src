using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool ChargeArrowInput { get; private set; }
    public bool ChargeArrowInputRelease { get; private set; }
    public bool AttackInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    public static event Action OnTriggerFungusDialog;

    [Header("Action Map Names")]
    [Tooltip("Name of the action map for toggling ingame menus.")]
    [SerializeField] private string InGameMenusActionMapName;
    [Tooltip("Name of the action map for Fungus input.")]
    [SerializeField] private string FungusActionMapName;

    [Header("Gameplay Input Settings")]
    [SerializeField] private float jumpInputHoldTime = 0.2f;
    [SerializeField] private float dashInputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions.FindActionMap(InGameMenusActionMapName).Enable();
        playerInput.actions.FindActionMap(FungusActionMapName).Enable();
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    #region Gameplay
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x); // > 0.5 => 1, < 0.5 = 0.
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
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

    public void OnChargeArrowInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChargeArrowInput = true;
            ChargeArrowInputRelease = false;
        }
        else if (context.canceled)
        {
            ChargeArrowInputRelease = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();
        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseChargeArrowInput() => ChargeArrowInput = false;

    public void UseAttackInput() => AttackInput = false;

    public void UseDashInput() => DashInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + jumpInputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + dashInputHoldTime)
        {
            DashInput = false;
        }
    }
    #endregion

    #region In Game Menus
    public void OnToggleInGameMenusUI(InputAction.CallbackContext context)
    {
        if (context.started && AllowInGameMenusToggle())
        {
            TogglePlayerLockedState();
            UIManagerScript.Instance.ToggleInGameMenusUI();
        }
    }

    public void OnInGameMenuSelect(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.InGameMenuSelect();
    }

    public void OnInGameMenuShiftLeft(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.ShiftLeft();
    }

    public void OnInGameMenuShiftRight(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.ShiftRight();
    }

    public void OnMoveInGameMenuSelectionUp(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.MoveInGameMenuSelectionUp();
    }

    public void OnMoveInGameMenuSelectionDown(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.MoveInGameMenuSelectionDown();
    }

    public void OnMoveInGameMenuSelectionLeft(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.MoveInGameMenuSelectionLeft();
    }

    public void OnMoveInGameMenuSelectionRight(InputAction.CallbackContext context)
    {
        if (context.started && InGameMenuManager.Instance != null)
            InGameMenuManager.Instance.MoveInGameMenuSelectionRight();
    }
    #endregion

    public void OnTriggerFungusDialogInput() => OnTriggerFungusDialog?.Invoke();

    private void TogglePlayerLockedState()
    {
        Player player = GetComponent<Player>();
        if (player != null)
        {
            player.ToggleLockState();
        }
    }

    private bool AllowInGameMenusToggle()
    {
        Player player = GetComponent<Player>();
        return player != null && !player.InAir();
    }
}
