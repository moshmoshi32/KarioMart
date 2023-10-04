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
    
    private GameState previousGameState;

    [HideInInspector]
    public LevelToLoad currentLevelLoaded;
    
    public static GameManager Instance;
    
    private PlayerInputManager playerInputManager;

    private LevelData currentLevelData;

    private int checkPointAmount = 0;

    private List<Checkpoints> currentCheckPoints = new List<Checkpoints>();

    [HideInInspector]
    public int amountPlayersFinished = 0;

    private UIManager uiManager;
    
    public Action<int> playerFinished;

    public Action<bool> pauseGame;

    public Action restartScene;

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
        pauseGame += PauseAction;
        restartScene += RestartCurrentLevel;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (uiManager.ShouldTick)
        {
            uiManager.updateTimer?.Invoke(Time.timeSinceLevelLoad);
        }
    }

    private void PauseAction(bool pause)
    {
        if (currentLevelLoaded == LevelToLoad.MainMenu) return;
        SetCurrentGameState(GameState.Paused);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        uiManager.ToggleTicking(false);
        uiManager.ToggleMenus(true);
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

    public void SwitchToSelectedScene(LevelToLoad levelToLoad)
    {
        SceneManager.LoadScene((int)levelToLoad);
        currentLevelLoaded = levelToLoad;
    }

    public void InitalizePreState(PlayerInputManager inputManager)
    {
        playerInputManager = inputManager;
        
        playerInputManager.onPlayerJoined += PlayerJoined;
        
        InitalizeGameUI();
    }

    private void InitalizeGameUI()
    {
        if (uiManager == null)
        {
            var gameUI = Resources.Load<UIManager>("Prefabs/GameUI");
            var currentUI = Instantiate(gameUI);
            uiManager = currentUI;
            DontDestroyOnLoad(uiManager);
            uiManager.ToggleTicking(false);
            uiManager.gameObject.SetActive(false);
        }
        else
        {
            uiManager.gameObject.SetActive(true);
        }
    }

    private void InitalizeGameState(int sceneIndex)
    {
        if (previousGameState == GameState.Paused)
        {
            Time.timeScale = 1;
            uiManager.ToggleTicking(true);
            return;
        }
        Debug.Log(sceneIndex);
        //Get the current level data
        currentLevelData = Resources.Load<LevelData>($"ScriptableObject/Level {sceneIndex - 1}");
        uiManager.gameObject.SetActive(true);
        uiManager.ToggleTicking(true);
        
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
        uiManager.ToggleTicking(true);
        uiManager.gameObject.SetActive(false);
    }

    public void SetCurrentGameState(GameState newGameState, int sceneIndex = -1)
    {
        previousGameState = CurrentGameState;
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;
            case GameState.Playing:
                InitalizeGameState(sceneIndex);
                break;
            case GameState.Paused:
                PauseGame();
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
        pauseGame -= PauseAction;
        restartScene -= RestartCurrentLevel;
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
