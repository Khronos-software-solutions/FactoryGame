using UnityEngine;

public enum ResourceType { Item, Fluid };
public class Resource : MonoBehaviour
{
    public string friendlyName;
    public string uiName;
    public string description;
    public string usageDescription;

    public ResourceType type;
}
