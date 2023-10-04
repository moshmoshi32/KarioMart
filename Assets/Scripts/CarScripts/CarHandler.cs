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

    private void Start()
    {
        InitalizeCar();
    }

    public void FixedUpdate()
    {
        movement.RotateHorizontally(inputManager.HorizontalMovementProperty);
        movement.MoveVertical(inputManager.VerticalMovementProperty);
    }

    private void InitalizeCar()
    {
        inputManager = new InputManager(input);
        inputManager.EnableInput();
        inputManager.SubscribeInputActions();
        rb = GetComponent<Rigidbody>();
        movement = new CarMovement(carInfo.maxSpeed, carInfo.rotationforce, rb, transform, carInfo.rotationRate, carInfo.acceleration);
        lapManager = new LapManager(this);
    }

    public void DisableInput()
    {
        inputManager.DisableInput();
    }
}
