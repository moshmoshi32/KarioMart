using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] 
    private List<CarInformationSO> avaliableCars = new List<CarInformationSO>();
    [Space]
    [Space]
    
    [HideInInspector]
    public GameState CurrentGameState;
    
    private GameState previousGameState;

    [HideInInspector]
    public LevelToLoad currentLevelLoaded;

    private LevelToLoad previousLevelLoaded;

    private LevelData currentLevelData;

    private int checkPointAmount = 0;

    private List<Checkpoints> currentCheckPoints = new List<Checkpoints>();

    [HideInInspector]
    public int amountPlayersFinished = 0;

    #region Actions
    
    public Action<int> playerFinished;

    public Action<CarHandler> playerPassedFinishLine;

    public Action<bool> pauseGame;

    public Action restartScene;
    #endregion

    private int currentGlobalLap;

    private List<CarHandler> carsPassed = new List<CarHandler>();

    private int playerWinnerIndex = -1;
    
    private float timeCompleted;

    #region Managers
    
    private PlayerInputManager playerInputManager;
    
    public UIManager UIManager { get; private set; }
    public TimerManager TimerManager { get; private set; }
    
    #endregion


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

        TimerManager = new TimerManager();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        LeaderBoard.InitalizeLeaderBoard();
    }

    private void Update()
    {
        if (!TimerManager.IsTimersListEmpty() && UIManager.ShouldTick)
        {
            TimerManager.TickTimer(Time.deltaTime);
        }
        if (UIManager.ShouldTick)
        {
            UIManager.updateTimer?.Invoke(Time.timeSinceLevelLoad);
        }
    }

    private void PopulateCarInformation(List<CarInformationSO> cars)
    {
        avaliableCars = cars;
    }
    private void PauseAction(bool pause)
    {
        if (currentLevelLoaded == LevelToLoad.MainMenu || CurrentGameState == GameState.Result) return;
        SetCurrentGameState(GameState.Paused);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        ChangeCursorMode(CursorLockMode.Confined, true);
        UIManager.ToggleTicking(false);
        UIManager.ToggleGameAndPausePanel(true);
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
        UIManager.UpdateLaps(currentGlobalLap);
    }

    private void ChangeCursorMode(CursorLockMode lockMode, bool visible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }

    public void SwitchToSelectedScene(LevelToLoad levelToLoad)
    {
        if (levelToLoad > Enum.GetValues(typeof(LevelToLoad)).Cast<LevelToLoad>().Last())
        {
            SceneManager.LoadScene((int)LevelToLoad.MainMenu);
            return;
        }
        SceneManager.LoadScene((int)levelToLoad);
        currentLevelLoaded = levelToLoad;
    }

    public void InitializePreState(PlayerInputManager inputManager, List<CarInformationSO> cars)
    {
        PopulateCarInformation(cars);
        playerInputManager = inputManager;
        
        playerInputManager.onPlayerJoined += PlayerJoined;
        
        InitializeGameUI();
    }

    private void InitializeGameUI()
    {
        if (UIManager == null)
        {
            var gameUI = Resources.Load<UIManager>("Prefabs/GameUI");
            var currentUI = Instantiate(gameUI);
            UIManager = currentUI;
            DontDestroyOnLoad(UIManager);
            UIManager.ToggleTicking(false);
            UIManager.gameObject.SetActive(false);
        }
        else
        {
            UIManager.gameObject.SetActive(true);
        }
    }

    private void InitializeGameState()
    {
        if (previousGameState == GameState.Paused)
        {
            Time.timeScale = 1;
            UIManager.ToggleTicking(true);
            ChangeCursorMode(CursorLockMode.Locked, false);
            return;
        }
        InitializeCurrentScene();
    }

    private void InitializeCurrentScene()
    {
        //Get the current level data
        UIManager.gameObject.SetActive(true);
        UIManager.ToggleTicking(true);
        
        if (!currentLevelData)
        {
            Debug.LogError("Couldn't find the data level!");
        }
    
        playerInputManager.JoinPlayer();
        playerInputManager.JoinPlayer();
        playerInputManager.JoinPlayer();

        InitializeTrack();
        UIManager.InitalizeGameUI(); 
    }

    private void InitializeTrack()
    {
        UIManager.InitalizeLapText(currentLevelData.maxLaps);
        amountPlayersFinished = 0;
        checkPointAmount = 0;
        currentGlobalLap = 1;
        carsPassed.Clear();
        currentCheckPoints.Clear();
        ChangeCursorMode(CursorLockMode.Locked, false);
        playerWinnerIndex = -1;
    }

    private void LoadMainMenu()
    {
        var test = Resources.Load("Prefabs/MainMenu") as GameObject;
        Instantiate(test);
        UIManager.ToggleTicking(true);
        UIManager.gameObject.SetActive(false);
        ChangeCursorMode(CursorLockMode.Confined, true);
    }

    public void SetCurrentGameState(GameState newGameState)
    {
        previousGameState = CurrentGameState;
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;
            case GameState.Playing:
                InitializeGameState();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.Result:
                EndGame();
                break;
        }
    }

    private void IncreasePlayerFinished(int playerWon)
    {
        if (timeCompleted == 0)
        {
            timeCompleted = Time.timeSinceLevelLoad;
        }
        playerWinnerIndex = playerWinnerIndex == -1 ? playerWon : playerWinnerIndex;
        amountPlayersFinished++;

        if (AllPlayersFinishedRace())
        {
            Debug.Log("Game done!");
            SetCurrentGameState(GameState.Result);
        }
    }

    private void EndGame()
    {
        Debug.Log($"Player{playerWinnerIndex + 1} won!");
        UIManager.VictoryScreen();
        UIManager.SetPlayerWon(playerWinnerIndex + 1);
        ChangeCursorMode(CursorLockMode.Confined, true);
    }

    private bool AllPlayersFinishedRace()
    {
        return amountPlayersFinished >= playerInputManager.playerCount;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousLevelLoaded = currentLevelLoaded;
        TimerManager.DestroyAllTimers();
        if (CurrentGameState == GameState.Restarted)
        {
            SetCurrentGameState(GameState.Playing);
            return;
        }
        switch (scene.buildIndex)
        {
            case 1:
                SetCurrentGameState(GameState.MainMenu);
                break;
            default:
                currentLevelData = Resources.Load<LevelData>($"ScriptableObject/Level {scene.buildIndex - 1}");
                SetCurrentGameState(GameState.Playing);
                break;
        }
    }
    public void SaveData(string playerName)
    {
        Debug.Log(playerName);
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Name is empty, returning...");
            return;
        }
        LeaderBoard.SavePlayerData(previousLevelLoaded, playerName, timeCompleted);
        timeCompleted = 0;
    }

    
    public void NextLevel()
    {
        SwitchToSelectedScene(++currentLevelLoaded);
    }
    
    private void RestartCurrentLevel()
    { 
        SwitchToSelectedScene(currentLevelLoaded);
    }

    private void PlayerJoined(PlayerInput input)
    {
        //Get a random car data
        var randomCarIndex = Random.Range(0, avaliableCars.Count);
        var carHandler = input.GetComponent<CarHandler>();
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public enum GameState
{
    None = 0,
    MainMenu = 1,
    Playing = 2,
    Paused = 3,
    Restarted = 4,
    Result = 5
}
