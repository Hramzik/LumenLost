using UnityEngine;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour
{
    private static CubeManager instance;
    private Dictionary<int, List<AppearingCube>> cubeDictionary = new Dictionary<int, List<AppearingCube>>();

    public static CubeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CubeManager");
                instance = obj.AddComponent<CubeManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public void RegisterCube(AppearingCube cube)
    {
        if (!cubeDictionary.ContainsKey(cube.triggerKey))
        {
            cubeDictionary.Add(cube.triggerKey, new List<AppearingCube>());
        }
        cubeDictionary[cube.triggerKey].Add(cube);
    }

    public void UnregisterCube(AppearingCube cube)
    {
        if (cubeDictionary.ContainsKey(cube.triggerKey))
        {
            cubeDictionary[cube.triggerKey].Remove(cube);
        }
    }

    public List<AppearingCube> GetCubesByKey(int key)
    {
        if (cubeDictionary.ContainsKey(key))
        {
            return new List<AppearingCube>(cubeDictionary[key]);
        }
        return new List<AppearingCube>();
    }
}