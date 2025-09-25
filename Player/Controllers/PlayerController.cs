using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected string UIActionMap;
    [SerializeField] protected EInputMode currentInputMode;
    
    [Header("UI Input Actions")]
    [SerializeField] protected InputActionReference IA_Back;

    protected PlayerInput playerInput;
    
    private InputDevice lastInputDevice;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (!playerInput)
        {
            Debug.LogError("PlayerController:Awake: Failed to get PlayerInput component.");
            return;
        }
        Cursor.visible = false;
    }

    protected virtual void OnEnable()
    {
        playerInput.onActionTriggered += HandleInput;
    }

    protected virtual void OnDisable()
    {
        playerInput.onActionTriggered -= HandleInput;
    }

    protected virtual void HandleInput(InputAction.CallbackContext context)
    {
        HandleInputDeviceChange(context);
        
        if (context.action == IA_Back.action)
        {
            HandleBackInput(context);
        }
    }

    private void HandleInputDeviceChange(InputAction.CallbackContext context)
    {
        InputDevice device = context.control.device;
        
        if (lastInputDevice == device)
            return;
        lastInputDevice = device;

        InputTypeEvent inputTypeEvent = new();
        switch (device)
        {
            case Gamepad:
            {
                inputTypeEvent.newInputType = EInputType.Gamepad;
                Cursor.visible = false;
                break;
            }
            case Keyboard:
            case Mouse:
            {
                inputTypeEvent.newInputType = EInputType.KeyboardAndMouse;
                Cursor.visible = currentInputMode == EInputMode.UI;
                break;
            }
        }

        if (inputTypeEvent.newInputType == EInputType.None)
            return;
        
        EventBus.Publish(inputTypeEvent);
    }
    
    private void HandleBackInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        
        InputActionEvent backInputActionEvent = new(context.action);
        EventBus.Publish(backInputActionEvent);
    }
}
