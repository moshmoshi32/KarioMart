using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGameScene(int scene)
    {
        GameManager.Instance.SwitchToSelectedScene((LevelToLoad)scene);
    }
}
