using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public static class JsonHelper
{
    public static string JsonUpperCamelCase(TextAsset json)
    {
        // Convert all JSON keys to upper camel case because I hate having to use non-abstract classes
        return Regex.Replace(json.text, @"""([a-z][a-z0-9]*)"":", m => "\"" + char.ToUpper(m.Groups[1].Value[0]) + m.Groups[1].Value[1..] + "\":");
    }
}

public abstract class ItemQuantityText
{
    // Describes a resource and an amount in id text form
    public abstract int Amount { get; }
    public abstract string ID { get; }
}

public abstract class SingleRecipeText
{
    // Describes a recipe's inputs outputs and processing time in text form
    public abstract List<ItemQuantityText> Inputs { get; }
    public abstract List<ItemQuantityText> Outputs { get; }
    public abstract float ProcessingTime { get; }
}

public abstract class RecipeFile
{
    public abstract Dictionary<string, List<SingleRecipeText>> Recipes { get; }
}

public class RecipeLoader
{
    private RecipeFile _serializedRecipes;
    private List<MachineType> _types;

    public List<Recipe> LoadRecipes(ResourceManager itemDB)
    {
        var types = Resources.LoadAll<MachineType>("Machines").ToList();
        var recipeFile = JsonHelper.JsonUpperCamelCase(Resources.Load<TextAsset>("Definitions/recipes"));
        if (recipeFile != null)
        {
            _serializedRecipes = JsonUtility.FromJson<RecipeFile>(recipeFile);
        }
        else
        {
            Debug.LogError("Failed to load recipes. Cause: file not found");
            return new List<Recipe>();
        }

        var recipes = new List<Recipe>();
        foreach (var item in _serializedRecipes.Recipes)
        {
            var type = types.Find(i => i.id == item.Key);
            if (type.id == null)
            {
                Debug.LogWarning($"Machine type \"{item.Key}\" not found. Skipping...");
                continue;
            }

            foreach (var recipe in item.Value)
            {
                var outputs =
                    recipe.Outputs.ToDictionary(output => itemDB.ByID(output.ID), output => output.Amount);

                var inputs =
                    recipe.Inputs.ToDictionary(input => itemDB.ByID(input.ID), input => input.Amount);


                var r = new Recipe
                {
                    machineType = type,
                    inputs = inputs,
                    processingTime = recipe.ProcessingTime
                };
                recipes.Add(r);
            }

            Debug.Log($"Loaded all recipes of type {item.Key}");
        }

        Debug.Log($"Done loading ({recipes.Count}) recipes");
        return recipes;
    }
}

public class RecipeManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    private List<Recipe> _recipes;

    private void Start()
    {
        var r = new RecipeLoader();
        _recipes = r.LoadRecipes(resourceManager);
    }

    public List<Recipe> ByMachineID(string machineID)
    {
        var machineType = Resources.LoadAll<MachineType>("Machines").ToList().Find(i => i.id == machineID); 
        return _recipes.FindAll(recipe => recipe.machineType == machineType);
    }

    public List<Recipe> ByInputID(string resourceID)
    {
        var resource = resourceManager.ByID(resourceID);
        return _recipes.FindAll(r => r.inputs.ContainsKey(resource));
    }

    public List<Recipe> ByOutputID(string resourceID)
    {
        var resource = resourceManager.ByID(resourceID);
        return _recipes.FindAll(r => r.outputs.ContainsKey(resource));
    }
}