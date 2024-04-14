using Godot;
using System;

public partial class KORecipe : AKitchenObject
{
    [Export] private bool on;
    [Export] private KitchenRoomController kitchenRoomController;

    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    protected override void InteractAction(Hand hand)
    {
        kitchenRoomController.ToggleRecipeRoom(on, true);
    }
}
