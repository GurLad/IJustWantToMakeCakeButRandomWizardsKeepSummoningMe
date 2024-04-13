using Godot;
using System;

public abstract partial class AKitchenObject : Node
{
    [Export] private Sprite2D sprite;

    public void Highlight(Hand hand)
    {
        // TBA
    }

    public void Leave(Hand hand)
    {
        // TBA
    }

    public abstract bool CanInteract(Hand hand);
    public abstract void Interact(Hand hand);
}
