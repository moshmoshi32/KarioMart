using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement
{
    private Rigidbody rb;
    private Transform carTransform;
    private float maxSpeed;
    private float currentSpeed;
    private float rotationSpeed;
    private float rotationRate;
    private float acceleration;

    public CarMovement(float _maxSpeed, float _rotationSpeed, Rigidbody _rb, Transform _carTransform, float _rotationRate, float _acceleration)
    {
        maxSpeed = _maxSpeed;
        rotationSpeed = _rotationSpeed;
        rb = _rb;
        carTransform = _carTransform;
        rotationRate = _rotationRate;
        acceleration = _acceleration;
    }

    public void MoveVertical(float movementAxis)
    {
        currentSpeed = rb.velocity.magnitude;
        
        if (movementAxis == 0) return;
        if (currentSpeed >= maxSpeed) return;
        Vector3 direction = movementAxis == 1 ? carTransform.forward : -carTransform.forward;
        rb.AddForce(direction * acceleration, ForceMode.Acceleration);
    }

    public void RotateHorizontally(float horizontalAxis)
    {
        rb.AddTorque(carTransform.up * (rotationSpeed * horizontalAxis), ForceMode.Acceleration);
    }

    public void SetNewMaxSpeed(float newMaxSpeed)
    {
        maxSpeed += newMaxSpeed;
    }
}
