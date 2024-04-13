using Godot;
using System;

public abstract partial class ATimedKitchenObject : AKitchenObject
{
    [Export] private float time;
    [Export] private AudioStream workingSFX;

    public override void Interact(Hand hand)
    {
        Play(workingSFX);
        hand.BeginTimedAction(this, time, () => { Play(finishSFX); InteractAction(hand); });
    }

    public void StopAction()
    {
        Play(null);
    }
}
