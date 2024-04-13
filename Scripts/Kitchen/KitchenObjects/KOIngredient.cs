using Godot;
using System;

public partial class KOIngredient : AKitchenObject
{
    [Export] private string name;

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsFull || hand.HasIngredient(name);
    }

    protected override void InteractAction(Hand hand)
    {
        hand.AddIngredient(name);
    }
}
