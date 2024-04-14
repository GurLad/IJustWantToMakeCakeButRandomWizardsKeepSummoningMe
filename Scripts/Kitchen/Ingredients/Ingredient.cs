using Godot;
using System;

public enum IngredientState { Normal = 0, Cut = 1, Burned = 2, Wet = 4 }

public class Ingredient
{
    public IngredientData Data;
    public IngredientState State = IngredientState.Normal;
    public int Count = 1;
    public string Name => Data.Name;

    public Ingredient() { }

    public Ingredient(IngredientData data)
    {
        Data = data;
    }

    public bool CanApplyState(IngredientState target)
    {
        return State == IngredientState.Normal && (Data.ValidStates & target) != 0;
    }

    public void ApplyState(IngredientState target)
    {
        State = target;
    }

    public Texture2D GetIcon()
    {
        return Data.Icons[State];
    }

    public override string ToString()
    {
        return Count + " X " + State.ToDisplayName() + Name;
    }
}
