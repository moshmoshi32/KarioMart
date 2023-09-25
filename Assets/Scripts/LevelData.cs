using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level/New Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public List<GameObject> startPoints = new List<GameObject>();
    public List<Transform> checkPoints = new List<Transform>();
    public Transform endPoint;
}
