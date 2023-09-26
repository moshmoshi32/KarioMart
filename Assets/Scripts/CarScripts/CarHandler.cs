using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarHandler : MonoBehaviour
{
    [Header("Car Properties")]
    [Range(10f, 100)]
    [SerializeField] private float acceleration;
    [Space] [SerializeField] private float maxSpeed;
    [Space]
    [Range(1, 360)]
    [SerializeField] private float rotationforce;

    [Space] [SerializeField] private PlayerInput input;
    
    private CarMovement movement;
    private Rigidbody rb;

    private InputManager inputManager;

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
        //Moving the car back and forward
        if (rb.velocity.magnitude >= maxSpeed) return;
        movement.MoveVertical(inputManager.VerticalMovementProperty);
    }

    private void InitalizeCar()
    {
        inputManager = new InputManager(input);
        inputManager.EnableInput();
        inputManager.SubscribeInputActions();
        rb = GetComponent<Rigidbody>();
        movement = new CarMovement(acceleration, rotationforce, rb, transform);
    }
}
