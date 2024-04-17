using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    public PlayerInputHandler GetInputHandler() => inputHandler;

    #region Unity Callbacks
    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }
    #endregion
}
