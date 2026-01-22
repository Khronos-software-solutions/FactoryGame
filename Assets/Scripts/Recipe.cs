using System;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public MachineType machineType;
    public Dictionary<Resource, int> inputs = new(); // Inputs and outputs, where the value is the amount (liters for fluids)
    public Dictionary<Resource, int> outputs = new();
    public float processingTime = 1f; // Time to process the recipe in seconds
    public string nameOverride; // A name override for the recipe.
                                // By default, the recipe name is the resulting resource, but this overrides this,
                                // for example if the recipe has multiple outputs or the product has multiple recipes.
}