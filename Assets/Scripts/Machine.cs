using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Loading;
using UnityEngine;

[System.Serializable]
public class Machine : MonoBehaviour
{
    public MachineType type;
    public float processingCounter = 0f;
    public MachineRecipe recipe;

    public Dictionary<Resource, int> contents;

    public int InsertResource(Resource resource, int amount) // Add resource to contents while respecting recipe and stackSize
    {
        if (!recipe.inputs.ContainsKey(resource))
        {
            return 0;
        }
        if (!contents.ContainsKey(resource))
        {
            if (amount >= resource.stackSize)
            {
                contents.Add(resource, resource.stackSize);
                return resource.stackSize;
            }
            contents.Add(resource, amount);
            return amount;
        }
        if (contents[resource] >= resource.stackSize)
        {
            return 0;
        }
        if (contents[resource] + amount > resource.stackSize)
        {
            var addedAmount = resource.stackSize - contents[resource];
            contents[resource] = resource.stackSize;
            return addedAmount;
        }
        contents[resource] += amount;
        return amount;
    }

    public bool CanProcess()
    {
        foreach (KeyValuePair<Resource, int> input in recipe.inputs)
        {
            if (!contents.ContainsKey(input.Key))
            {
                return false;
            }
            if (contents[input.Key] == 0)
            {
                return false;
            }
            // Not done
        }
        return true;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}

