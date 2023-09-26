using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    #region Variables
    private PlayerInput input;

    private float verticalMovement;

    public float VerticalMovementProperty
    {
        get => verticalMovement;
        private set => verticalMovement = value;
    }
    
    private float horizontalMovement;

    public float HorizontalMovementProperty
    {
        get => horizontalMovement;
        private set => horizontalMovement = value;
    }
    #endregion

    public InputManager(PlayerInput _input)
    {
        input = _input;
    }
    public void SubscribeInputActions()
    {
        input.currentActionMap.FindAction("VerticalMovement").performed += GetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").performed += GetHorizontalMovementAxis;
        
        input.currentActionMap.FindAction("VerticalMovement").canceled += ResetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").canceled += ResetHorizontalMovementAxis;
    }
    
    public void UnsubscribeInputActions()
    {
        input.currentActionMap.FindAction("VerticalMovement").performed -= GetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").performed -= GetHorizontalMovementAxis;
        
        input.currentActionMap.FindAction("VerticalMovement").canceled -= ResetVerticalMovementAxis;
        input.currentActionMap.FindAction("HorizontalMovement").canceled -= ResetHorizontalMovementAxis;
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
