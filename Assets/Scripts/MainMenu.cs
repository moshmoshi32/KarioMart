using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> leaderBoardLayouts = new();
    public void LoadGameScene(int scene)
    {
        GameManager.Instance.SwitchToSelectedScene((LevelToLoad)scene);
    }
    
    public void LoadAllLeaderBoards()
    {
        GameManager.Instance.UIManager.LoadAllLeaderBoards(leaderBoardLayouts);
    }

    public void RemoveAllText()
    {
        GameManager.Instance.UIManager.DisposeOfTextObjects();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
