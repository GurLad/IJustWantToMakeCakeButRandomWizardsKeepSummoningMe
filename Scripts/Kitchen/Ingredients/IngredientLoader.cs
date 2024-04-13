using Godot;
using System;
using System.Collections.Generic;

public partial class IngredientLoader : Node
{
    public override void _Ready()
    {
        base._Ready();
        List<IngredientLoaderObject> objects = new List<IngredientLoaderObject>();
        foreach (var child in GetChildren())
        {
            if (child is IngredientLoaderObject ingredientLoader)
            {
                objects.Add(ingredientLoader);
            }
        }
        IngredientController.LoadData(objects);
    }
}
