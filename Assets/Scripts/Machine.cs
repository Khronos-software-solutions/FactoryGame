#nullable enable
using System.Collections.Generic;
using System.Linq;
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
    public GameObject instance;

    public int
        InsertResource(Resource resource, int amount) // Add resource to contents while respecting recipe and stackSize
    {
        if (recipe == null) return 0;
        if (!recipe.inputs.ContainsKey(resource)) return 0; // Check if the resource is needed for the recipe
        if (!contents.TryGetValue(resource, out var content)) // Check if the key exists
        {
            if (amount >= resource.stackSize)
            {
                contents.Add(resource, resource.stackSize);
                return resource.stackSize;
            }

            contents.Add(resource, amount);
            return amount;
        }

        if (content >= resource.stackSize) return 0; // Check if contents are full
        if (contents[resource] + amount >
            resource.stackSize) // Handle case where inserting the normal amount results in overflowing
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
            if (!contents.TryGetValue(input.Key, out var content)) return false; // Check if key exists
            if (content < recipe.inputs[input.Key]) return false; // Check if there are enough for the recipe
        }

        return recipe.outputs.Where(output => outputs.ContainsKey(output.Key))
            .All(output => outputs[output.Key] + output.Value <= output.Key.stackSize);
    }

    public void Process()
    {
        foreach (KeyValuePair<Resource, int> output in recipe.outputs)
        {
            outputs.TryAdd(output.Key, 0); // Add key value pair if it does not exist
            outputs[output.Key] += output.Value; // Add outputs
        }
    }

    private void Update()
    {
        if ((!CanProcess() && !processOverride) || recipe == null) return;
        processingCounter += 1f * Time.deltaTime; // Update processing counter
        if (processingCounter >= recipe.processingTime) Process();
    }
}