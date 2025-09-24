using UnityEngine.InputSystem;

public class InputActionEvent
{
    private readonly InputAction inputAction;

    public InputActionEvent(InputAction inputAction)
    {
        this.inputAction = inputAction;
    }
    
    public InputAction GetInputAction() => inputAction;
}
