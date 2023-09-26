using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private bool isEnd = false;
    private void Start()
    {
        GameManager.Instance.CheckPointAmount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnd) return;
        var player = other.GetComponent<CarHandler>();
        if (player)
        {
            if (isEnd && player.LapManager.CheckPointsPassed >= GameManager.Instance.CheckPointAmount)
            {
                player.LapManager.IncreaseLap();
                Debug.Log("Lap increased!");
                return;
            }
            Debug.Log("Checkpoint hit!");
            player.LapManager.IncreaseCheckPointAmount();
        }
    }
}
