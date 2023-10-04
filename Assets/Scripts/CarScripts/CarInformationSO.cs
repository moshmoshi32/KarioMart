using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Car", menuName = "Car/New Car Data", order = 1)]
public class CarInformationSO : ScriptableObject
{
    [Header("Car Properties")]
    [Range(10f, 100)]
    public float acceleration;
    [Space] public float maxSpeed;
    [Space]
    [Range(1, 360)]
    public float rotationforce;

    public float rotationRate = 0.6f;
}
