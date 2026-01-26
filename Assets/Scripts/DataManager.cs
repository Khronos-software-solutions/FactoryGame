using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public List<Resource> Resources { get; private set; }
    public List<MachineRecipe> Recipes { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private T LoadJson<T>(string fileName) where T : new()
    {

        string filePath = Path.Combine(Application.streamingAssetsPath, $"{fileName}.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Could not find {fileName}.json at {filePath}");
            return (T)Activator.CreateInstance(typeof(T));
        }
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(json);
    }
}