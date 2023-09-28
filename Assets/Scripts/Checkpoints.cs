using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private List<CarHandler> playersPassed = new List<CarHandler>(); 
    [SerializeField] private bool isEnd = false;

    private void Start()
    {
        if (!isEnd)
        {
            GameManager.Instance.CheckPointAmount++;
        }

        GameManager.Instance.AddCheckPointToList(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<CarHandler>();
        if (player)
        {
            if (isEnd && player.LapManager.CheckPointsPassed >= GameManager.Instance.CheckPointAmount - 1)
            {
                player.LapManager.IncreaseLap();
                Debug.Log("Lap increased!");
                return;
            }

            if (playersPassed.Contains(player) || isEnd) return;
            player.LapManager.IncreaseCheckPointAmount();
            playersPassed.Add(player);
        }
    }

    public void ResetCheckPoint(CarHandler playerPassed)
    {
        playersPassed.Remove(playerPassed);
    }
}
