using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGameScene()
    {
        GameManager.Instance.SwitchToSelectedScene((LevelToLoad)2);
    }
}
