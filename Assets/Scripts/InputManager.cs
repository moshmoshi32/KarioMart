using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    #region Variables
    private GameControls gameControls = new();

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
    public void SubscribeInputActions()
    {
        gameControls.Car.VerticalMovement.performed += GetVerticalMovementAxis;
        gameControls.Car.HorizontalMovement.performed += GetHorizontalMovementAxis;

        gameControls.Car.VerticalMovement.canceled += ResetVerticalMovementAxis;
        gameControls.Car.HorizontalMovement.canceled += ResetHorizontalMovementAxis;
    }
    
    public void UnsubscribeInputActions()
    {
        gameControls.Car.VerticalMovement.performed -= GetVerticalMovementAxis;
        gameControls.Car.HorizontalMovement.performed -= GetHorizontalMovementAxis;
        
        gameControls.Car.VerticalMovement.canceled -= ResetVerticalMovementAxis;
        gameControls.Car.HorizontalMovement.canceled -= ResetHorizontalMovementAxis;
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
        gameControls.Car.Enable();
    }

    public void DisableInput()
    {
        gameControls.Car.Disable();
    }
}
