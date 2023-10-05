using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class LeaderBoard
{
    private static string filePath = $"{Application.persistentDataPath}/PlayerData.json";
    private static List<PlayerData> allPlayersData;
    
    public static void SavePlayerData(LevelToLoad levelCompleted, string name, float timeFinished)
    {
        allPlayersData = new List<PlayerData>();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            allPlayersData = JsonConvert.DeserializeObject<List<PlayerData>>(json);
            Debug.Log("File existed");
        }
        PlayerData data = new PlayerData
        {
            levelCompleted = levelCompleted,
            playerName = name,
            timeFinished = timeFinished
        };
        
        Debug.Log(allPlayersData);
        
        allPlayersData.Add(data);
        string output = JsonConvert.SerializeObject(allPlayersData);
        Debug.Log(output);
        File.WriteAllText(filePath, output);
    }
}


public struct PlayerData
{
    public string playerName;
    public float timeFinished;
    public LevelToLoad levelCompleted;
}