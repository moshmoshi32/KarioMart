using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> leaderBoardLayouts = new List<GameObject>();
    public void LoadGameScene(int scene)
    {
        GameManager.Instance.SwitchToSelectedScene((LevelToLoad)scene);
    }
    
    public void LoadAllLeaderBoards()
    {
        GameManager.Instance.uiManager.LoadAllLeaderBoards(leaderBoardLayouts);
    }

    public void RemoveAllText()
    {
        GameManager.Instance.uiManager.DisposeOfTextObjects();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
