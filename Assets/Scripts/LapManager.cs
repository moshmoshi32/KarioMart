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

    public void IncreaseLap()
    {
        CurrentLap++;
    }

    public void ResetLaps()
    {
        currentLap = 0;
    }
}
