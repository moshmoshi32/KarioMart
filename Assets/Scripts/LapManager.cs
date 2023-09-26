using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager
{
    private int currentLap = 0;

    public int CurrentLap
    {
        get => currentLap;
        private set => currentLap = value;
    }

    private int checkPointAmount = 0;

    public int CheckPointsPassed
    {
        get => checkPointAmount;
        private set => checkPointAmount = value;
    }

    public void IncreaseCheckPointAmount()
    {
        CheckPointsPassed++;
    }
    public void IncreaseLap()
    {
        CurrentLap++;
    }

    public void ResetLaps()
    {
        currentLap = 0;
    }
}
