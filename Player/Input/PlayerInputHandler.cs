using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public Vector2 RawWarpDirectionInput { get; private set; }
    public Vector2Int WarpDirectionInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool ChargeBowInput { get; private set; }
    public bool ChargeBowInputRelease { get; private set; }
    public bool AttackInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool HoldWarpInput { get; private set; }
    public bool HoldWarpInputStop { get; private set; }
    public bool ThunderInput { get; private set; }
    public bool AirGlideInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool AdvanceInteractionInput {  get; private set; }

    [Header("Action Map Names")]
    [Tooltip("Name of the action map for toggling ingame menus.")]
    [SerializeField] private string InGameMenusActionMapName;

    [Header("Gameplay Input Settings")]
    [SerializeField] private float jumpInputHoldTime = 0.2f;
    [SerializeField] private float dashInputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (!playerInput)
        {
            Debug.LogError("PlayerInputHandler:Awake: Failed to get PlayerInput component.");
        }
    }

    private void Start()
    {
        playerInput.actions.FindActionMap(InGameMenusActionMapName).Enable();
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

    public void OnChargeBowInput(InputAction.CallbackContext context)
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
            DashInput = false;
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();
        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void OnWarpInput(InputAction.CallbackContext context)
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

    public void OnWarpDirectionInput(InputAction.CallbackContext context)
    {
        RawWarpDirectionInput = context.ReadValue<Vector2>();
        WarpDirectionInput = Vector2Int.RoundToInt(RawWarpDirectionInput.normalized);
    }

    public void OnThunderInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ThunderInput = true;
        }
        if (context.canceled)
        {
            ThunderInput = false;
        }
    }

    public void OnAirGlideInput(InputAction.CallbackContext context)
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            InteractInput = true;
        if (context.canceled)
            InteractInput = false;
    }

    public void OnAdvanceInteractionInput(InputAction.CallbackContext context)
    {
        if (context.started)
            AdvanceInteractionInput = true;
        if (context.canceled)
            AdvanceInteractionInput = false;
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseChargeBowInput() => ChargeBowInput = false;

    public void UseAttackInput() => AttackInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseHoldWarpInput() => HoldWarpInput = false;

    public void UseThunderInput() => ThunderInput = false;

    public void UseInteractInput() => InteractInput = false;
    public void UseAdvanceInteractionInput() => AdvanceInteractionInput = false;

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
        }
    }

    public void OnToggleDebugMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerPawn player = FindObjectOfType<PlayerPawn>();
            if (player != null)
            {
                player.EnableAllLockedStates();
            }
        }
    }
    #endregion

    private bool AllowInGameMenusToggle()
    {
        PlayerPawn player = GetComponent<PlayerPawn>();
        return player != null && !player.InAir();
    }
}
