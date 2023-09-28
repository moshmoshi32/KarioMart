using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level/New Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public List<Vector3> startPoints = new List<Vector3>();
    public int maxLaps;
}
