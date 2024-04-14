using Godot;
using System;

public partial class KOQuit : AKitchenObject
{
    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    protected override void InteractAction(Hand hand)
    {
        GetTree().Quit();
    }
}
