using Godot;
using System;
using System.Collections.Generic;

public partial class KOBowl : AKitchenObject
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty;
    }

    public override void Interact(Hand hand)
    {
        ingredients.AddRange(hand.TakeAllIngredients());
    }
}
