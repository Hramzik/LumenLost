using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Settings")]
    [SerializeField] private CubeSettings cubeSettings;
    [SerializeField] private List<LevelData> levels;
    
    [Header("Debug")]
    [SerializeField] private int currentLevelIndex = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteCurrentLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count) { LoadScene(); return; }
        if (currentLevelIndex == levels.Count) { LoadFinalScene(); return; }

        // Restart game
        currentLevelIndex = 0;
        LoadScene();
    }

    public void ReloadCurrentLevel()
    {
        if (currentLevelIndex < levels.Count) LoadScene();
        else                                  LoadFinalScene();

        BlindnessCube.canStartCoroutine = true;
        BlindnessCube.effectCoroutine = null;
        BlindnessCube.coroutineStarter = null;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void LoadFinalScene()
    {
        SceneManager.LoadScene("FinalLevel");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            InitializeLevel();
        }
    }

    private void InitializeLevel()
    {
        LevelUtilities.ClearCurrentLevel();
        
        if (currentLevelIndex < levels.Count)
        {
            LevelData levelData = levels[currentLevelIndex];
            LevelUtilities.LoadLevelFromData(levelData, cubeSettings);
        }
        else
        {
            Debug.LogError("Missing level data!");
        }
    }
}