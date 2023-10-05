using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CarHandler : MonoBehaviour
{
    [Space] [SerializeField] private PlayerInput input;

    [SerializeField] private CarInformationSO carInfo;
    
    private CarMovement movement;
    private Rigidbody rb;

    private InputManager inputManager;

    private LapManager lapManager;

    private bool carInitalized = false;
    public LapManager LapManager
    {
        get => lapManager;
        private set => lapManager = value;
    }

    public InputManager PlayerInputManager
    {
        private set => inputManager = value;
        get => inputManager;
    }
    public void FixedUpdate()
    {
        if (!carInitalized) return;
        movement.RotateHorizontally(inputManager.HorizontalMovementProperty);
        movement.MoveVertical(inputManager.VerticalMovementProperty);
    }

    public void InitalizeCar(CarInformationSO carDetails)
    {
        inputManager = new InputManager(input);
        inputManager.EnableInput();
        inputManager.SubscribeInputActions();
        rb = GetComponent<Rigidbody>();
        carInfo = carDetails;
        movement = new CarMovement(carInfo.maxSpeed, carInfo.rotationforce, rb, transform, carInfo.rotationRate, carInfo.acceleration);
        lapManager = new LapManager(this);
        carInitalized = true;
    }

    public void ChangeMaxSpeed(float speedToAdd)
    {
        movement.SetNewMaxSpeed(speedToAdd);
    }

    public void DisableInput()
    {
        inputManager.DisableInput();
    }
}
