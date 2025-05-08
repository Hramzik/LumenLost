using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public List<CubeData> cubes = new List<CubeData>();
}

[System.Serializable]
public class CubeData
{
    public Vector3 position;
    public CubeController.CubeType type;
    public Color color;
}
