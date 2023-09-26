using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level/New Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public List<Vector3> startPoints = new List<Vector3>();
    public List<Vector3> checkPoints = new List<Vector3>();
    public Transform endPoint;
}
