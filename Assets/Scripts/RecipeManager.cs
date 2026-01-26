using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class RecipeFileLayout
{
      public inputs
}

public abstract class RecipeFile
{
      public Dictionary<string, List<Recipe>> recipes;
}

public class RecipeLoader
{
      private RecipeFileLayout _serializedRecipes;
      private List<MachineType> _types;
      

      public List<Recipe> LoadRecipes()
      {
            var types = Resources.LoadAll<MachineType>("Machines").ToList();
            var recipeFile = Resources.Load<TextAsset>("Definitions/recipes");
            if (recipeFile != null) _serializedRecipes = JsonUtility.FromJson<RecipeFileLayout>(recipeFile.text);
            else Debug.LogError("Failed to load recipes");

            List<Recipe> recipes = new List<Recipe>();
            foreach (var item in _serializedRecipes.recipes)
            {
                  var type = types.Find(i => i.id == item.Key);
                  foreach (Recipe recipe in item.Value)
                  {
                        var r = recipe;
                        r.machineType = type;
                        recipes.Add(r);
                  }
            }

            return recipes;
      }
}

public class RecipeManager : MonoBehaviour
{
      public List<Recipe> 
}