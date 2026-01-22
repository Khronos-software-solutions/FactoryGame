using System;
using UnityEngine;

public enum ResourceType { Item, Fluid };
public enum GroupType { Dust, Raw, Processed, Product }

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

public class ResourceFileItem
{
    public string id;
    public string name;
    public string description;
    public string usageDescription;
    public int stackSize;
    public string type;
}
public class ResourceFileFluid
{
    public string id;
    public string name;
    public string description;
    public string usageDescription;
    public bool processTemperature;
    public string type;

}