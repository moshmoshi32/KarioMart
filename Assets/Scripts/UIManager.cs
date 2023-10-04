using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI lapText;
    [Space]
    [Header("Panel References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gamePanel;

    private int maxLaps;

    public Action<float> updateTimer;

    public bool ShouldTick { get; private set; }


    private void UpdateTimer(float time)
    {
        timerText.text = time.ToString("n2");
    }

    public void ToggleTicking(bool value)
    {
        ShouldTick = value;
    }

    private void OnEnable()
    {
        updateTimer += UpdateTimer;
    }
    
    private void OnDisable()
    {
        updateTimer += UpdateTimer;
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.SwitchToSelectedScene(LevelToLoad.MainMenu);
        ToggleMenus(false);
    }

    public void RestartLevel()
    {
        GameManager.Instance.restartScene?.Invoke();
        ToggleMenus(false);
    }

    public void ToggleMenus(bool toggle)
    {
        pauseMenu.SetActive(toggle);
        gamePanel.SetActive(!toggle);

        if (!toggle)
        {
            GameManager.Instance.SetCurrentGameState(GameState.Playing);
        }
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
}

