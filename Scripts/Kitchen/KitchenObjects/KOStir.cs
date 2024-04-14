using Godot;
using System;

public partial class KOStir : ATimedKitchenObject
{
    [Export] private KitchenController kitchenController;
    [Export] private KOBowl bowl;

    public override bool CanInteract(Hand hand)
    {
        return !bowl.IsEmpty;
    }

    protected override void InteractAction(Hand hand)
    {
        kitchenController.AddCakeIngredients(bowl.TakeAllIngredients());
    }
}
