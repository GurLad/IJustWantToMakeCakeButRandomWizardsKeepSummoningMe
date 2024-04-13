using Godot;
using System;
using System.Collections.Generic;

public partial class IngredientsDisplay : Container
{
    [Export] private PackedScene sceneIngredientIcon;

    private List<IngredientIcon> currentIcons = new List<IngredientIcon>();

    public void Regenerate(List<Ingredient> ingredients)
    {
        // Lazy, just clear & recreate all
        currentIcons.ForEach(a => a.QueueFree());
        currentIcons.Clear();

        foreach (var ingredient in ingredients)
        {
            IngredientIcon newIcon = sceneIngredientIcon.Instantiate<IngredientIcon>();
            newIcon.Update(ingredient);
            currentIcons.Add(newIcon);
        }
    }
}
