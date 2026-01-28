using System.Collections.Generic;
using System.Linq;
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
    
    public List<Resource> LoadResources()
    {
        var itemFile = Resources.Load<TextAsset>("Definitions/items");
        var fluidFile = Resources.Load<TextAsset>("Definitions/fluids");
        
        if (itemFile != null) _items = JsonUtility.FromJson<ItemWrapper>(itemFile.text).items;
        else Debug.LogError("Failed to load items");
        if (fluidFile != null) _fluids = JsonUtility.FromJson<FluidWrapper>(fluidFile.text).fluids;
        else Debug.LogError("Failed to load fluids");

        var resources = _items.Select(item => item.ToResource()).ToList();
        resources.AddRange(_fluids.Select(item => item.ToResource()));

        return resources;
    }
}

public class ResourceManager : MonoBehaviour
{
    private List<Resource> _resources; // We make the resource list private because the only way we want them to be accessed is via `ByID()`

    private void Awake()
    {
        var rl = new ResourceLoader();
        _resources = rl.LoadResources();
    }

    public Resource ByID(string id) // Get a resource from all by searching by it for its ID
    {
        var r = _resources.Find(item => item.id == id);
        return r ?? new Resource();
    }

    public int GetCount()
    {
        return _resources.Count;
    }
}