using Godot;
using System;

public abstract partial class AKitchenObject : Node
{
    [ExportGroup("Internal")]
    [Export] private Sprite2D sprite;
    [Export] private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        base._Ready();
        RectangleShape2D trueShape = new RectangleShape2D();
        trueShape.Size = sprite.Texture.GetSize();
        collisionShape.Shape = trueShape;
        collisionShape.Position = trueShape.Size / 2f;
    }

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
