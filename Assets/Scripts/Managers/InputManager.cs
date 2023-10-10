using UnityEngine.InputSystem;

public class InputManager
{
    #region Variables
    private PlayerInput input;

    public float VerticalMovementProperty { get; private set; }

    public float HorizontalMovementProperty { get; private set; }

    public int GetPlayerIndex() => input.playerIndex;
    #endregion

    public InputManager(PlayerInput _input)
    {
        input = _input;
    }
    public void SubscribeInputActions()
    {
        input.currentActionMap.FindAction("VerticalMovement").performed += GetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").performed += GetHorizontalMovementAxis;

        input.currentActionMap.FindAction("Pause").started += GetPauseButtonDown;
        
        input.currentActionMap.FindAction("VerticalMovement").canceled += ResetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").canceled += ResetHorizontalMovementAxis;
        
    }

    private void GetPauseButtonDown(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.pauseGame?.Invoke(ctx.started);
    }

    private void GetVerticalMovementAxis(InputAction.CallbackContext ctx)
    {
        VerticalMovementProperty = ctx.ReadValue<float>();
    }
    
    private void GetHorizontalMovementAxis(InputAction.CallbackContext ctx)
    {
        HorizontalMovementProperty= ctx.ReadValue<float>();
    }

    private void ResetVerticalMovementAxis(InputAction.CallbackContext ctx)
    {
        VerticalMovementProperty = 0;
    }
    
    private void ResetHorizontalMovementAxis(InputAction.CallbackContext ctx)
    {
        HorizontalMovementProperty = 0;
    }

    
    public void EnableInput()
    {
        input.currentActionMap.Enable();
    }

    public void DisableInput()
    {
        input.currentActionMap.Disable();
    }
}
