using Godot;
using System;
using System.Collections.Generic;

public class IngredientData
{
    public string Name;
    public IngredientState ValidStates;
    public Dictionary<IngredientState, Texture2D> Icons { get; private set; } = new Dictionary<IngredientState, Texture2D>();

    public IngredientData(IngredientLoaderObject loaderObject)
    {
        Name = loaderObject.Name;
        Icons.Add(IngredientState.Normal, loaderObject.baseSprite);
        AddIcon(IngredientState.Burned, loaderObject.burnedSprite);
        AddIcon(IngredientState.Cut, loaderObject.cutSprite);
        AddIcon(IngredientState.Wet, loaderObject.wetSprite);
    }

    private void AddIcon(IngredientState state, Texture2D icon)
    {
        if (icon != null)
        {
            Icons.Add(state, icon);
            ValidStates |= state;
        }
    }
}
