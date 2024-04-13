using Godot;
using System;

public partial class KOIngredient : AKitchenObject
{
    [Export] private string name;

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsFull || hand.HasIngredient(name);
    }

    public override void Interact(Hand hand)
    {
        hand.AddIngredient(name);
    }
}
