using UnityEngine;

[System.Serializable]
public class CubeTypeSettings
{
    public CubeController.CubeType type;
    public string prefabPath;
    public Color defaultColor;
}

[CreateAssetMenu(fileName = "CubeSettings", menuName = "Level Editor/Cube Settings")]
public class CubeSettings : ScriptableObject
{
    public CubeTypeSettings[] cubeTypeSettings;

    public CubeTypeSettings GetSettings(CubeController.CubeType type)
    {
        foreach (var setting in cubeTypeSettings)
        {
            if (setting.type == type) return setting;
        }
        return null;
    }
}