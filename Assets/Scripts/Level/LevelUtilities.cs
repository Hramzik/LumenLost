using UnityEditor;
using UnityEngine;

public static class LevelUtilities
{
    #if UNITY_EDITOR
    public static void SaveLevelToData(LevelData levelData)
    {
        levelData.cubes.Clear();

        var cubes = Object.FindObjectsByType<CubeController>(FindObjectsSortMode.None);
        foreach (var cube in cubes)
        {
            CubeData data = new CubeData
            {
                position = cube.transform.position,
                type = cube.cubeType,
                color = cube.GetComponent<Renderer>().sharedMaterial.color
            };

            levelData.cubes.Add(data);
        }

        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        Debug.Log("Level saved!");
    }
    #endif

    public static void LoadLevelFromData(LevelData levelData, CubeSettings settings)
    {
        ClearCurrentLevel();

        foreach (var cubeData in levelData.cubes)
        {
            var cubeSettings = settings.GetSettings(cubeData.type);
            GameObject cube = Object.Instantiate(
                Resources.Load<GameObject>(cubeSettings.prefabPath)
            );
            cube.transform.position = cubeData.position;

            if (cubeData.type == CubeController.CubeType.Portal) continue;

            Renderer renderer = cube.GetComponent<Renderer>();
            Material newMaterial = new Material(renderer.sharedMaterial) { color = cubeData.color };
            renderer.sharedMaterial = newMaterial;
        }
    }

    public static void ClearCurrentLevel()
    {
        var oldCubes = Object.FindObjectsByType<CubeController>(FindObjectsSortMode.None);
        foreach (var cube in oldCubes) Object.DestroyImmediate(cube.gameObject);
    }
}