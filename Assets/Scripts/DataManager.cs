using System.Collections.Generic;
using UnityEngine;

public class ItemWrapper<T>
{
    
}

public class ResourceLoader : MonoBehaviour
{
    public List<ResourceFileItem> items = new();
    public List<ResourceFileFluid> fluids = new();

    void Start()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        var jsonFile = Resources.Load<TextAsset>("Definitions/items");

        if (jsonFile != null)
        {
            items = JsonUtility.FromJson<List<ResourceFileItem>>(jsonFile.text).items;
            fluids = JsonUtility.FromJson<List<ResourceFileFluid>>(jsonFile.text).fluid;
        }
        else
        {
            Debug.LogError("Failed to load items.json");
        }
    }
}