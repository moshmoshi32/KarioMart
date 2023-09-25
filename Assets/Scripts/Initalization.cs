using UnityEngine;
using UnityEngine.InputSystem;

public class Initalization
{
    [RuntimeInitializeOnLoadMethod]
    private static void Initalize()
    {
        GameObject go = new GameObject();
        go.name = "GameManager";
        var playerInputManager = go.AddComponent<PlayerInputManager>();
        var gameManager = go.AddComponent<GameManager>();
        gameManager.InitalizePreState(playerInputManager);
        
        playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        playerInputManager.playerPrefab = Resources.Load("Car") as GameObject;
        
        gameManager.SwitchToSelectedScene(1);
        
    }
}
