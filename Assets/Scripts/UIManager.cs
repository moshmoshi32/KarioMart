using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gamePanel;

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
}

