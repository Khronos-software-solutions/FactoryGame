using System;
using UnityEngine;

public enum ResourceType { Item, Fluid };
public enum GroupType { Dust, Raw, Processed, Product, Other }

public class Resource
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
    
    public Resource ToResource()
    {
        var r = new Resource();
        r.id = this.id;
        r.uiName = this.name;
        r.description = this.description;
        r.usageDescription = this.usageDescription;
        r.stackSize = this.stackSize;
        r.type = ResourceType.Fluid;
        if (type == "raw") r.group = GroupType.Raw;
        else if(type == "dust") r.group = GroupType.Dust;
        else if(type == "processed") r.group = GroupType.Processed;
        else if(type == "product") r.group = GroupType.Product;
        else r.group = GroupType.Other;
        return r;
    }
}
public class ResourceFileFluid
{
    public string id;
    public string name;
    public string description;
    public string usageDescription;
    public int stackSize;
    public string type;

    public Resource ToResource()
    {
        var r = new Resource();
        r.id = this.id;
        r.uiName = this.name;
        r.description = this.description;
        r.usageDescription = this.usageDescription;
        r.stackSize = this.stackSize;
        r.type = ResourceType.Fluid;
        if (type == "raw") r.group = GroupType.Raw;
        else if(type == "dust") r.group = GroupType.Dust;
        else if(type == "processed") r.group = GroupType.Processed;
        else if(type == "product") r.group = GroupType.Product;
        else r.group = GroupType.Other;
        return r;
    }
}