using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class LeaderBoard
{
    private static bool dataLoaded = false;
    private static string filePath = $"{Application.persistentDataPath}/PlayerData.json";
    private static List<PlayerData> allPlayersData;

    public static void InitalizeLeaderBoard()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("The file does not exist! Leaderboard data won't be loaded!");
            return;
        }
        string json = File.ReadAllText(filePath);
        allPlayersData = JsonConvert.DeserializeObject<List<PlayerData>>(json);
        dataLoaded = true;
    }
    
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
        output += "\n";
        Debug.Log(output);
        File.WriteAllText(filePath, output);
        dataLoaded = true;
    }

    public static List<PlayerData> LoadDataForSpecificLevel(LevelToLoad level)
    {
        if (!dataLoaded)
        {
            Debug.LogWarning("There is no data to load! Returning...");
            return null;
        }
        List<PlayerData> playerdatas = new List<PlayerData>();
        foreach (var playerData in allPlayersData)
        {
            if (playerData.levelCompleted == level)
            {
                Debug.Log(playerData.playerName);
                playerdatas.Add(playerData);
            }
        }
        return playerdatas;
    }
}


public struct PlayerData : IComparable<PlayerData>
{
    public string playerName;
    public float timeFinished;
    public LevelToLoad levelCompleted;

    public int CompareTo(PlayerData other)
    {
        var playerNameComparison = string.Compare(playerName, other.playerName, StringComparison.Ordinal);
        if (playerNameComparison != 0) return playerNameComparison;
        var timeFinishedComparison = timeFinished.CompareTo(other.timeFinished);
        if (timeFinishedComparison != 0) return timeFinishedComparison;
        return levelCompleted.CompareTo(other.levelCompleted);
    }

    public int SortByFloatAscending(float x, float y)
    {
        return y.CompareTo(x);
    }
}