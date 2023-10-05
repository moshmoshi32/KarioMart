using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager
{
    private List<Timer> timers = new List<Timer>();
    public void CreateTimer(float time, float valueToReset, Action<float, CarHandler> resetFunction, CarHandler car)
    {
        Timer timer = new Timer(time, valueToReset, resetFunction, car);
        timers.Add(timer);
    }

    public bool IsTimersListEmpty()
    {
        return timers.Count <= 0;
    }

    public void TickTimer(float deltaTime)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if(timers[i].Tick(deltaTime))
            {
                timers.Remove(timers[i]);
            }
        }

        return;
        foreach (var timer in timers)
        {
            if (timer == null) return; 
            if(timer.Tick(deltaTime))
            {
                timers.Remove(timer);
            }
        }
    }
}
