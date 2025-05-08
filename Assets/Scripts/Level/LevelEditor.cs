#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    [SerializeField] private CubeSettings cubeSettings;
    private LevelData currentLevel;

    private CubeController.CubeType selectedCubeType = CubeController.CubeType.Basic;
    private static bool isPlacingCubes = false;
    private bool useCustomColor = false;
    private Color selectedColor = Color.white;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        cubeSettings = (CubeSettings)EditorGUILayout.ObjectField("Cube Settings", cubeSettings, typeof(CubeSettings), false);
        currentLevel = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevel, typeof(LevelData), false);

        if (GUILayout.Button("Create New Level"))
        {
            currentLevel = CreateInstance<LevelData>();
            AssetDatabase.CreateAsset(currentLevel, "Assets/Levels/NewLevel.asset");
            AssetDatabase.SaveAssets();
        }

        if (currentLevel == null || cubeSettings == null) return;

        selectedCubeType = (CubeController.CubeType)EditorGUILayout.EnumPopup("Cube Type", selectedCubeType);
        if (selectedCubeType != CubeController.CubeType.Void)
        {
            useCustomColor = EditorGUILayout.Toggle("Set Custom Color", useCustomColor);

            if (useCustomColor) selectedColor = EditorGUILayout.ColorField("Cube Color", selectedColor);
            else                selectedColor = cubeSettings.GetSettings(selectedCubeType).defaultColor;
        }

        if (isPlacingCubes)
        {
            CubePlacer.SetTemplateCube(cubeSettings.GetSettings(selectedCubeType), selectedColor);
            if (GUILayout.Button("Stop Placing"))
            {
                CubePlacer.StopPlacing();
                isPlacingCubes = false;
            }
        }
        else
        {
            if (GUILayout.Button("Add Cubes to Scene"))
            {
                CubePlacer.SetTemplateCube(cubeSettings.GetSettings(selectedCubeType), selectedColor);
                isPlacingCubes = true;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button ("Save Level")) LevelUtilities.SaveLevelToData(currentLevel);
            if (GUILayout.Button ("Load Level")) LevelUtilities.LoadLevelFromData(currentLevel, cubeSettings);
            if (GUILayout.Button("Clear Level")) LevelUtilities.ClearCurrentLevel();
        }
    }

    private void SaveLevel()
    {
        currentLevel.cubes.Clear();

        var cubes = FindObjectsByType<CubeController>(FindObjectsSortMode.None);
        foreach (var cube in cubes)
        {
            CubeData data = new CubeData
            {
                position = cube.transform.position,
                type = cube.cubeType,
                color = cube.GetComponent<Renderer>().sharedMaterial.color
            };
            currentLevel.cubes.Add(data);
        }

        EditorUtility.SetDirty(currentLevel);
        AssetDatabase.SaveAssets();
        Debug.Log("Level saved!");
    }

    private void LoadLevel()
    {
        ClearLevel();

        foreach (var cubeData in currentLevel.cubes)
        {
            GameObject cube = Instantiate(Resources.Load<GameObject>(cubeSettings.GetSettings(cubeData.type).prefabPath));

            Renderer renderer = cube.GetComponent<Renderer>();
            Material newMaterial = new Material(renderer.sharedMaterial) { color = cubeData.color };
            renderer.sharedMaterial = newMaterial;
            cube.transform.position = cubeData.position;
        }
    }

    private void ClearLevel()
    {
        var oldCubes = FindObjectsByType<CubeController>(FindObjectsSortMode.None);
        foreach (var cube in oldCubes) DestroyImmediate(cube.gameObject);
    }

    private void OnDestroy()
    {
        CubePlacer.StopPlacing();
        isPlacingCubes = false;
    }
}
#endif