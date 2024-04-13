using Godot;
using System;

public partial class IngredientLoader : Node
{
    [Export] private IngredientLoaderObject[] objects;

    public override void _Ready()
    {
        base._Ready();
        IngredientController.LoadData(objects);
    }
}
