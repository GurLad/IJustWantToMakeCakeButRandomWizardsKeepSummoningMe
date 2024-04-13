using Godot;
using System;

public abstract partial class AKitchenObject : Node
{
    [ExportGroup("Internal")]
    [Export] private Sprite2D sprite;
    [Export] private CollisionShape2D collisionShape;
    [Export] private Material outlineMaterial;

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
        sprite.Material = outlineMaterial;
    }

    public void Leave(Hand hand)
    {
        sprite.Material = null;
    }

    public abstract bool CanInteract(Hand hand);
    public abstract void Interact(Hand hand);
}
