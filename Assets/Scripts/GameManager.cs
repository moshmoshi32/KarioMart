using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private List<CarInformationSO> avaliableCars = new List<CarInformationSO>();
    [Space]
    [Space]
    
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

    #region Actions
    
    public Action<int> playerFinished;

    public Action<CarHandler> playerPassedFinishLine;

    public Action<bool> pauseGame;

    public Action restartScene;
    #endregion

    private int currentGlobalLap;

    private List<CarHandler> carsPassed = new List<CarHandler>();

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
        playerPassedFinishLine += IncreasePlayersPassedGoal;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (uiManager.ShouldTick)
        {
            uiManager.updateTimer?.Invoke(Time.timeSinceLevelLoad);
        }
    }

    public void PopulateCarInformation(List<CarInformationSO> cars)
    {
        avaliableCars = cars;
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

    private void IncreasePlayersPassedGoal(CarHandler carPassed)
    {
        AddPlayerToLapFinished(carPassed);
        Debug.Log($"{carsPassed.Count} / {playerInputManager.playerCount}");
        if (carsPassed.Count >= playerInputManager.playerCount)
        {
            IncreaseLapUI();
            carsPassed.Clear();
        }
    }

    private void AddPlayerToLapFinished(CarHandler car)
    {
        if (!carsPassed.Contains(car))
        {
           carsPassed.Add(car); 
        }
    }

    public void ResetAllCheckPoints(CarHandler player)
    {
        foreach (var checkpoint in currentCheckPoints)
        {
            checkpoint.ResetCheckPoint(player);
        }
    }

    private void IncreaseLapUI()
    {
        currentGlobalLap++;
        uiManager.UpdateLaps(currentGlobalLap);
    }

    public void SwitchToSelectedScene(LevelToLoad levelToLoad)
    {
        SceneManager.LoadScene((int)levelToLoad);
        currentLevelLoaded = levelToLoad;
    }

    public void InitalizePreState(PlayerInputManager inputManager, List<CarInformationSO> cars)
    {
        PopulateCarInformation(cars);
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
        uiManager.InitalizeLapText(currentLevelData.maxLaps);
        amountPlayersFinished = 0;
        checkPointAmount = 0;
        currentGlobalLap = 1;
        carsPassed.Clear();
        currentCheckPoints.Clear();
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
        if (AllPlayersFinishedRace())
        {
            Debug.Log("Game done!");
            RestartCurrentLevel();
        }
    }

    private bool AllPlayersFinishedRace()
    {
        return amountPlayersFinished >= playerInputManager.playerCount;
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

    [ContextMenu("Save")]
    public void TestSaveData()
    {
        LeaderBoard.SavePlayerData(currentLevelLoaded, "Test", 15.0f);
    }

    private void RestartCurrentLevel()
    {
        SwitchToSelectedScene(currentLevelLoaded);
    }

    private void PlayerJoined(PlayerInput input)
    {
        //Get a random car data
        //TODO: Fix so that you can actually pick a car
        var randomCarIndex = Random.Range(0, avaliableCars.Count);
        var carHandler = input.GetComponent<CarHandler>();
        Debug.Log(randomCarIndex);
        carHandler.InitalizeCar(avaliableCars[randomCarIndex]);
        input.SwitchCurrentControlScheme($"Keyboard{input.playerIndex + 1}", Keyboard.current);
        input.transform.position = currentLevelData.startPoints[input.playerIndex];
    }

    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= PlayerJoined;
        pauseGame -= PauseAction;
        restartScene -= RestartCurrentLevel;
        playerPassedFinishLine -= IncreasePlayersPassedGoal;
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
