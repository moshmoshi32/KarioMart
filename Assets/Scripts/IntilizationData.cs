using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Initalization Data", menuName = "Initalize/New Initlization Data", order = 1)]
public class IntilizationData : ScriptableObject
{
    public LevelToLoad levelToLoad;
}

public enum LevelToLoad {
    Init = 0,
    MainMenu = 1,
    LevelOne = 2,
    LevelTwo = 3,
    LevelThree = 4,
}
