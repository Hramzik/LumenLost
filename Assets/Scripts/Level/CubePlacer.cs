#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CubePlacer
{
    private static CubeTypeSettings cubeSettings;
    private static Color cubeColor;

    public static void SetTemplateCube(CubeTypeSettings settings, Color color)
    {
        cubeSettings = settings;
        cubeColor = color;
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                
                Vector3 roundedPosition = new Vector3(
                    Mathf.Round(hitPoint.x),
                    Mathf.Round(hitPoint.y),
                    Mathf.Round(hitPoint.z)
                );
                
                PlaceCube(roundedPosition);
            }

            e.Use();
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            StopPlacing();
        }
    }

    private static void PlaceCube(Vector3 position)
    {
        CubeController[] allCubes = GameObject.FindObjectsByType<CubeController>(FindObjectsSortMode.None);

        if (cubeSettings.type == CubeController.CubeType.Void)
        {
            foreach (CubeController cube in allCubes)
            {
                if (Vector3.Distance(cube.transform.position, position) < 0.1f) GameObject.DestroyImmediate(cube.gameObject);
            }
            return;
        }

        foreach (CubeController cube in allCubes)
        {
            if (Vector3.Distance(cube.transform.position, position) < 0.1f) return;
        }

        GameObject cubeGO = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(cubeSettings.prefabPath)) as GameObject;
        cubeGO.transform.position = position;

        if (cubeSettings.type == CubeController.CubeType.Portal) return;
        Renderer renderer = cubeGO.GetComponent<Renderer>();
        Material newMat = new Material(renderer.sharedMaterial);
        newMat.color = cubeColor;
        renderer.sharedMaterial = newMat;
    }

    public static void StopPlacing()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
#endif