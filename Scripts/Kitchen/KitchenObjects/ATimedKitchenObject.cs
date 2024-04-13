using Godot;
using System;

public abstract partial class ATimedKitchenObject : AKitchenObject
{
    [Export] private float time;

    public override void Interact(Hand hand)
    {
        hand.BeginTimedAction(time, InteractAction);
    }

    protected abstract void InteractAction(Hand hand);
}
