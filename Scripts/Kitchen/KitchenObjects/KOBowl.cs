using Godot;
using System;
using System.Collections.Generic;

public partial class KOBowl : ATimedKitchenObject
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty;
    }

    protected override void InteractAction(Hand hand)
    {
        ingredients.AddRange(hand.TakeAllIngredients());
    }
}
