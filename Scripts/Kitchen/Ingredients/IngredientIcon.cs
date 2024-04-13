using Godot;
using System;

public partial class IngredientIcon : Node
{
    // Exports
    [Export] private Label countLabel;
    [Export] private TextureRect icon;

    public void Update(Ingredient ingredient)
    {
        icon.Texture = ingredient.GetIcon();
        countLabel.Text = ingredient.Count.ToString();
        countLabel.Visible = ingredient.Count > 1;
    }
}
