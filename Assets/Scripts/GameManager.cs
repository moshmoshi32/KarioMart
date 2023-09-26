using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameState CurrentGameState;
    
    public static GameManager Instance;
    
    private PlayerInputManager playerInputManager;

    private List<LevelData> levelData = new List<LevelData>();
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
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SwitchToSelectedScene(LevelToLoad levelToLoad)
    {
        SceneManager.LoadScene((int)levelToLoad);
    }

    public void InitalizePreState(PlayerInputManager inputManager)
    {
        playerInputManager = inputManager;
        
        playerInputManager.onPlayerJoined += PlayerJoined;
    }

    private void InitalizeGameState()
    {
        playerInputManager.JoinPlayer();
        playerInputManager.JoinPlayer(controlScheme: "Keyboard2", pairWithDevice: Keyboard.current);
    }

    private void LoadMainMenu()
    {
        var test = Resources.Load("MainMenu") as GameObject;
        Instantiate(test);
    }

    private void SetCurrentGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.MainMenu:
                LoadMainMenu();
                break;
            case GameState.Playing:
                InitalizeGameState();
                break;
        }
        //TODO: Add logic for switching states
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 1:
                SetCurrentGameState(GameState.MainMenu);
                
                break;
            case 2:
                SetCurrentGameState(GameState.Playing);
                break;
        }
    }

    private void PlayerJoined(PlayerInput input)
    {
        Debug.Log("Heyo!");
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
