using Godot;
using System;
using System.Collections.Generic;

public partial class KOBowl : ATimedKitchenObject
{
    public List<Ingredient> Ingredients { private get; set; } = new List<Ingredient>();
    public bool IsEmpty => Ingredients.Count <= 0;

    [Export] private KitchenController kitchenController;

    public override void _Ready()
    {
        base._Ready();
        kitchenController?.ConnectBowl(this);
    }

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty;
    }

    protected override void InteractAction(Hand hand)
    {
        Ingredients.AddRange(hand.TakeAllIngredients());
    }

    public List<Ingredient> TakeAllIngredients()
    {
        List<Ingredient> result = new List<Ingredient>(Ingredients);
        Ingredients.Clear();
        return result;
    }
}
