using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI leaderBoardEntryTemplate;
    [Space]
    [Header("Panel References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject namePanel;

    [Space] [Header("Input References")] [SerializeField]
    private TMP_InputField nameField;
    private int maxLaps;

    public Action<float> updateTimer;

    private List<GameObject> textObjects = new List<GameObject>();

    public bool ShouldTick { get; private set; }


    private void UpdateTimer(float time)
    {
        timerText.text = time.ToString("n2");
    }

    public void ToggleTicking(bool value)
    {
        ShouldTick = value;
    }

    public void SaveData()
    {
        GameManager.Instance.SaveData(nameField.text);
        nameField.text = "";
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.SwitchToSelectedScene(LevelToLoad.MainMenu);
        ToggleGameAndPausePanel(false);
        DisableVictoryScreen();
    }

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
        DisableVictoryScreen();
    }

    public void RestartLevel()
    {
        GameManager.Instance.restartScene?.Invoke();
        ToggleGameAndPausePanel(false);
        DisableVictoryScreen();
        GameManager.Instance.SetCurrentGameState(GameState.Restarted);
    }

    private void DisableVictoryScreen()
    {
        namePanel.SetActive(false);
    }

    public void VictoryScreen()
    {
        gamePanel.SetActive(false);
        namePanel.SetActive(true);
    }

    public void InitalizeGameUI()
    {
        gamePanel.SetActive(true);
        namePanel.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleGameAndPausePanel(bool toggle)
    {
        pauseMenu.SetActive(toggle);
        gamePanel.SetActive(!toggle);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.Instance.SetCurrentGameState(GameState.Playing);
    }

    public void UpdateLaps(int newLap)
    {
        lapText.text = $"{newLap} / {maxLaps}";
        Debug.Log("updated laps!");
    }

    public void InitalizeLapText(int maxLap)
    {
        maxLaps = maxLap;
        lapText.text = $" 1 / {maxLap}";
    }

    private void LoadDataFromLeaderBoard(int level, List<GameObject> layouts)
    { 
       var leaderBoardData = LeaderBoard.LoadDataForSpecificLevel((LevelToLoad)level + 1);
       if (leaderBoardData == null)
       {
           Debug.Log("No data!");
           return;
       }
        /*
         https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
         This sort method was taken from this webpage.
         It calls a LINQ function and orders the time by the lowest float to the highest float
         and then converts it to a list.
        */
       List<PlayerData> playerData = leaderBoardData.OrderBy(x => x.timeFinished).ToList();
       
       foreach (var data in playerData)
       {
           var currentTextObject = Instantiate(leaderBoardEntryTemplate, layouts[level - 1].transform, false);
           currentTextObject.text = $"Name: {data.playerName}\nTime: {data.timeFinished:n2}";
           textObjects.Add(currentTextObject.gameObject);
       }
    }
    

    public void DisposeOfTextObjects()
    {
        foreach (var text in textObjects)
        {
            Destroy(text);
        }
    }

    public void LoadAllLeaderBoards(List<GameObject> layouts)
    {
        for (int i = 0; i <= 3; i++)
        {
            LoadDataFromLeaderBoard(i, layouts);
        }
    }
    
    private void OnEnable()
    {
        updateTimer += UpdateTimer;
    }
    
    private void OnDisable()
    {
        updateTimer += UpdateTimer;
    }
}

