#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Machine : MonoBehaviour
{
    public MachineType type;
    public float processingCounter = 0f;
    public MachineRecipe? recipe;
    public Power powerContents;
    public Dictionary<Resource, int> contents = new();
    public Dictionary<Resource, int> outputs = new();

    public bool processOverride; // Temp
    private GameObject? instance;

    public int InsertResource(Resource resource, int amount) // Add resource to contents while respecting recipe and stackSize
    {
        if (recipe == null) return 0;
        if (!recipe.inputs.ContainsKey(resource)) return 0; // Check if the resource is needed for the recipe
        if (!contents.ContainsKey(resource)) // Check if the key exists
        {
            if (amount >= resource.stackSize)
            {
                contents.Add(resource, resource.stackSize);
                return resource.stackSize;
            }
            contents.Add(resource, amount);
            return amount;
        }
        if (contents[resource] >= resource.stackSize) return 0; // Check if contents are full
        if (contents[resource] + amount > resource.stackSize) // Handle case where inserting the normal amount results in overflowing
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
        if (recipe == null) return false;
        // Check for all recipe inputs if there are enough resources stored
        foreach (KeyValuePair<Resource, int> input in recipe.inputs)
        {
            if (!contents.ContainsKey(input.Key)) return false; // Check if key exists
            if (contents[input.Key] < recipe.inputs[input.Key]) return false; // Check if there are enough for the recipe
        }
        foreach (KeyValuePair<Resource, int> output in recipe.outputs) // Check if finishing processing will result in overflow
        {
            if (outputs.ContainsKey(output.Key))
            {
                if (outputs[output.Key] + output.Value > output.Key.stackSize)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Process()
    {
        foreach (KeyValuePair<Resource, int> output in recipe.outputs)
        {
            if (!outputs.ContainsKey(output.Key)) outputs.Add(output.Key, 0); // Add key value pair if it does not exist
            outputs[output.Key] += output.Value; // Add outputs
        }
    }

    void Start()
    {
        instance = Instantiate(type.prefab);
    }

    void Update()
    {
        Debug.Log(recipe);
        if ((CanProcess() || processOverride) && recipe != null)
        {
            processingCounter += 1f * Time.deltaTime; // Update processing counter
            instance.transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime));
            if (processingCounter >= recipe.processingTime)
            {
                Process();
            }
        }
    }
}
