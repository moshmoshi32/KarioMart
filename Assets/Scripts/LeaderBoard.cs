using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LeaderBoard
{
    private static List<PlayerData> allPlayersData = new List<PlayerData>();
    
    public static void SavePlayerData(LevelToLoad levelCompleted, string name, float timeFinished)
    {
        PlayerData data = new PlayerData
        {
            levelCompleted = levelCompleted,
            playerName = name,
            timeFinished = timeFinished
        };
        
        allPlayersData.Add(data);
        string output = JsonConvert.SerializeObject(allPlayersData);
        Debug.Log(output);
    }
}


public struct PlayerData
{
    public string playerName;
    public float timeFinished;
    public LevelToLoad levelCompleted;
}