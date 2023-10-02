using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

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
}

