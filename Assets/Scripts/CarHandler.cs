using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private CarMovement movement;
    private Rigidbody rb;

    private InputManager inputManager;

    private bool initalized;

    private void Start()
    {
        InitalizeCar();
    }

    public void FixedUpdate()
    {
        if (!initalized) return;
        movement.RotateHorizontally(inputManager.HorizontalMovementProperty);
        //Moving the car back and forward
        if (rb.velocity.magnitude >= maxSpeed) return;
        movement.MoveVertical(inputManager.VerticalMovementProperty);
    }

    private void InitalizeCar()
    {
        inputManager = GameManager.Instance.InputManager;
        rb = GetComponent<Rigidbody>();
        movement = new CarMovement(acceleration, rotationforce, rb, transform);
        initalized = true;
    }
}
