using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameState CurrentGameState;

    [HideInInspector]
    public LevelToLoad currentLevelLoaded;
    
    public static GameManager Instance;
    
    private PlayerInputManager playerInputManager;

    private LevelData currentLevelData;

    private int checkPointAmount = 0;

    private List<Checkpoints> currentCheckPoints = new List<Checkpoints>();

    public Action<int> playerFinished;

    public int amountPlayersFinished = 0;

    public int CheckPointAmount
    {
        get => checkPointAmount;
        set => checkPointAmount = value;
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInputManager = playerInputManager == null ? GetComponent<PlayerInputManager>() : playerInputManager;
        playerFinished += IncreasePlayerFinished;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public int GetMaxLaps() => currentLevelData.maxLaps;

    public void AddCheckPointToList(Checkpoints checkpoint)
    {
        currentCheckPoints.Add(checkpoint);
    }

    public void ResetAllCheckPoints(CarHandler player)
    {
        foreach (var checkpoint in currentCheckPoints)
        {
            checkpoint.ResetCheckPoint(player);
        }
    }

    public void ClearCheckPointList()
    {
        currentCheckPoints.Clear();
    }

    public void SwitchToSelectedScene(LevelToLoad levelToLoad)
    {
        SceneManager.LoadScene((int)levelToLoad);
        currentLevelLoaded = levelToLoad;
    }

    public void InitalizePreState(PlayerInputManager inputManager)
    {
        playerInputManager = inputManager;
        
        playerInputManager.onPlayerJoined += PlayerJoined;
    }

    private void InitalizeGameState(int sceneIndex)
    {
        Debug.Log(sceneIndex);
        //Get the current level data
        currentLevelData = Resources.Load<LevelData>($"ScriptableObject/Level {sceneIndex - 1}");
        if (!currentLevelData)
        {
            Debug.LogError("Couldnt find the data level!");
        }
        playerInputManager.JoinPlayer();
        playerInputManager.JoinPlayer();
        amountPlayersFinished = 0;
        checkPointAmount = 0;
    }

    private void LoadMainMenu()
    {
        var test = Resources.Load("Prefabs/MainMenu") as GameObject;
        Instantiate(test);
    }

    private void SetCurrentGameState(GameState newGameState, int sceneIndex = -1)
    {
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                playerFinished -= IncreasePlayerFinished;
                break;
            case GameState.Playing:
                InitalizeGameState(sceneIndex);
                break;
        }
        //TODO: Add logic for switching states
    }

    private void IncreasePlayerFinished(int playerIndex)
    {
        amountPlayersFinished++;
        Debug.Log($"Player {playerIndex + 1} finished at time: {Time.timeSinceLevelLoad}");
        if (amountPlayersFinished >= playerInputManager.playerCount)
        {
            Debug.Log("Game done!");
            RestartCurrentLevel();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 1:
                SetCurrentGameState(GameState.MainMenu);
                break;
            default:
                SetCurrentGameState(GameState.Playing, scene.buildIndex);
                break;
        }
    }

    private void RestartCurrentLevel()
    {
        SwitchToSelectedScene(currentLevelLoaded);
    }

    private void PlayerJoined(PlayerInput input)
    {
        input.SwitchCurrentControlScheme($"Keyboard{input.playerIndex + 1}", Keyboard.current);
        input.transform.position = currentLevelData.startPoints[input.playerIndex];
    }

    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= PlayerJoined;
    }
}

public enum GameState
{
    None = 0,
    MainMenu = 1,
    Playing = 2,
    Paused = 3,
    Result = 4
}
