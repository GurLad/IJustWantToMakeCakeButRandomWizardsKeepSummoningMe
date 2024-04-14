using Godot;
using System;
using System.Collections.Generic;

public partial class KOBowl : ATimedKitchenObject
{
    public bool IsEmpty => ingredients.Count <= 0;

    private List<Ingredient> ingredients { get; } = new List<Ingredient>();

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty;
    }

    protected override void InteractAction(Hand hand)
    {
        ingredients.AddRange(hand.TakeAllIngredients());
    }

    public List<Ingredient> TakeAllIngredients()
    {
        List<Ingredient> result = new List<Ingredient>(ingredients);
        ingredients.Clear();
        return result;
    }
}
