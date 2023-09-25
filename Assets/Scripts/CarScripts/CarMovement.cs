using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement
{
    private Rigidbody rb;
    private Transform carTransform;
    private float speed;
    private float rotationSpeed;

    public CarMovement(float _speed, float _rotationSpeed, Rigidbody _rb, Transform _carTransform)
    {
        speed = _speed;
        rotationSpeed = _rotationSpeed;
        rb = _rb;
        carTransform = _carTransform;
    }

    public void MoveVertical(float movementAxis)
    {
        if (movementAxis == 0) return;
        Vector3 direction = movementAxis == 1 ? carTransform.forward : -carTransform.forward;
        rb.AddForce(direction * speed, ForceMode.Acceleration);
    }

    public void RotateHorizontally(float horizontalAxis)
    {
        rb.AddTorque(carTransform.up * (0.5f * rotationSpeed * horizontalAxis));
    }
}
