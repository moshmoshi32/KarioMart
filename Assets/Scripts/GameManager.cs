using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameState CurrentGameState;
    
    public static GameManager Instance;
    public InputManager InputManager;
    
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
    /// <summary>
    /// Loads the selected scene either with a name or an sceneIndex.
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <param name="sceneName"></param>
    public void SwitchToSelectedScene(int sceneNumber = -1, string sceneName = "")
    {
        if (sceneNumber == -1)
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        SceneManager.LoadScene(sceneNumber);
    }

    public void InitalizePreState(PlayerInputManager inputManager)
    {
        playerInputManager = inputManager;
        
        playerInputManager.onPlayerJoined += PlayerJoined;
    }

    private void InitalizeGameState()
    {
        /*
        InputManager = new InputManager();
        InputManager.EnableInput();
        InputManager.SubscribeInputActions();
        */

        playerInputManager.JoinPlayer();
        playerInputManager.JoinPlayer(controlScheme: "Keyboard2", pairWithDevice: Keyboard.current);
    }

    private void SetCurrentGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                InitalizeGameState();
                break;
        }
        //TODO: Add logic for switching states
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            SetCurrentGameState(GameState.MainMenu);
        }

        if (scene.buildIndex == 2)
        {
            SetCurrentGameState(GameState.Playing);
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
