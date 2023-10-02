using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager
{
    private CarHandler player;
    private int currentLap = 0;

    public int CurrentLap
    {
        get => currentLap;
        private set => currentLap = value;
    }

    public int CheckPointsPassed { get; private set; } = 0;

    public LapManager(CarHandler _player)
    {
        player = _player;
    }

    public void IncreaseCheckPointAmount()
    {
        CheckPointsPassed++;
    }
    public void IncreaseLap()
    {
        GameManager.Instance.ResetAllCheckPoints(player);
        CurrentLap++;
        CheckPointsPassed = 0;
        if (currentLap >= GameManager.Instance.GetMaxLaps())
        {
            GameManager.Instance.playerFinished?.Invoke(player.PlayerInputManager.GetPlayerIndex());
            player.DisableInput();
        }
    }

    public void ResetLaps()
    {
        currentLap = 0;
    }
}
