using UnityEngine;

public enum ResourceType { Item, Fluid };
public enum GroupType { Ore, Dust, Raw, Processed }

public class Resource : MonoBehaviour
{
    public string id;
    public string uiName;
    public string description;
    public string usageDescription;
    public int stackSize;
    public ResourceType type;
    public GroupType group;
}
