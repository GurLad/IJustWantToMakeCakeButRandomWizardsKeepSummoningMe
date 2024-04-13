using Godot;
using System;

public abstract partial class AKitchenObject : Sprite2D
{
    [Export] private string Tooltip;
    [ExportGroup("Internal")]
    [Export] private Area2D area;
    [Export] private CollisionShape2D collisionShape;
    [Export] private Material outlineMaterial;

    public override void _Ready()
    {
        base._Ready();
        RectangleShape2D trueShape = new RectangleShape2D();
        trueShape.Size = Texture.GetSize();
        collisionShape.Shape = trueShape;
        collisionShape.Position = trueShape.Size / 2f;
        // Blegh
        area.MouseEntered += () => { if (Hand.Current != null) Hand.Current.EnterKitchenObject(this); };
        area.MouseExited += () => { if (Hand.Current != null) Hand.Current.LeaveKitchenObject(this); };
    }

    public void Enter(Hand hand)
    {
        Material = outlineMaterial;
    }

    public void Leave(Hand hand)
    {
        Material = null;
    }

    public abstract bool CanInteract(Hand hand);
    public abstract void Interact(Hand hand);
}
