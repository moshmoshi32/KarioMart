using UnityEngine;
using UnityEngine.InputSystem;

public class Initalization
{
    private static IntilizationData data;
    [RuntimeInitializeOnLoadMethod]
    private static void Initalize()
    {
        data = Resources.Load<IntilizationData>("InitalizationData");
        GameObject go = new GameObject();
        go.name = "GameManager";
        var playerInputManager = go.AddComponent<PlayerInputManager>();
        var gameManager = go.AddComponent<GameManager>();
        gameManager.InitalizePreState(playerInputManager);
        
        playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        playerInputManager.playerPrefab = Resources.Load("Prefabs/Car") as GameObject;
        
        gameManager.SwitchToSelectedScene(data.levelToLoad);
    }
}
