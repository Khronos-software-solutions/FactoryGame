using System.Collections.Generic;
using UnityEngine;

public abstract class ItemWrapper
{
    public List<ResourceFileItem> items;
}

public abstract class FluidWrapper
{
    public List<ResourceFileFluid> fluids; 
}

public class ResourceLoader
{
    private List<ResourceFileItem> _items = new();
    private List<ResourceFileFluid> _fluids = new();
    
    public readonly List<Resource> resources = new();

    public void LoadResources()
    {
        var itemFile = Resources.Load<TextAsset>("Definitions/items");
        var fluidFile = Resources.Load<TextAsset>("Definitions/fluids");
        if (itemFile != null) _items = JsonUtility.FromJson<ItemWrapper>(itemFile.text).items;
        else Debug.LogError("Failed to load items");
        if (fluidFile != null) _fluids = JsonUtility.FromJson<FluidWrapper>(fluidFile.text).fluids;
        else Debug.LogError("Failed to load fluids");

        foreach (var item in _items)
        {
            resources.Add(item.ToResource());
        }

        foreach (var item in _fluids)
        {
            resources.Add(item.ToResource());
        }
    }
}

public class ResourceManager : MonoBehaviour
{
    public List<Resource> resources;

    void Start()
    {
        var rl = new ResourceLoader();
        rl.LoadResources();
        resources = rl.resources;
    }

    public Resource ByID(string id) // Get a resource from all by searching by it for its ID
    {
        var r = resources.Find(item => item.id == id);
        return r ?? new Resource();
    }

    public int GetCount()
    {
        return resources.Count;
    }
}