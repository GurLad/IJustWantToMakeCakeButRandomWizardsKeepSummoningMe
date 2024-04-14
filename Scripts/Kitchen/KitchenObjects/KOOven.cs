using Godot;
using System;

public partial class KOOven : ATimedKitchenObject
{
    [Export] private KitchenController kitchenController;

    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    protected override void InteractAction(Hand hand)
    {
        kitchenController.Kill();
        SceneController.Current.TransitionToScene("EndScreen");
    }
}
