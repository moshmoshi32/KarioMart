using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float currentTime;
    private float negateAmount;
    private CarHandler carOwner;
    private Action<float, CarHandler> carResetFunction;

    public Timer(float time, float negateAmount, Action<float, CarHandler> _carResetFunction, CarHandler car)
    {
        currentTime = time;
        carResetFunction = _carResetFunction;
        this.negateAmount = negateAmount;
        carOwner = car;
    }

    public bool Tick(float deltaTime)
    {
        Debug.Log("Ticking...");
        currentTime -= deltaTime;
        if (currentTime <= 0)
        {
            carResetFunction?.Invoke(negateAmount, carOwner);
            return true;
        }

        return false;
    }
}
