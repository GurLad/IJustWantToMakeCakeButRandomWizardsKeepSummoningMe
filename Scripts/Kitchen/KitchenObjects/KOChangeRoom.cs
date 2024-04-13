using Godot;
using System;

public partial class KOChangeRoom : AKitchenObject
{
    [Export] private int direction;
    [Export] private KitchenRoomController kitchenRoomController;

    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    public override void Interact(Hand hand)
    {
        kitchenRoomController.MoveRoom(direction);
    }
}
