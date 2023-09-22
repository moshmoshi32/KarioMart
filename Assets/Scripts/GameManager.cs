using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameState CurrentGameState;
    
    public static GameManager Instance;
    public InputManager InputManager;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitalizeGameState();
    }

    private void InitalizeGameState()
    {
        InputManager = new InputManager();
        InputManager.EnableInput();
        InputManager.SubscribeInputActions();
    }

    public void SetCurrentGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
        //TODO: Add logic for switching states
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
