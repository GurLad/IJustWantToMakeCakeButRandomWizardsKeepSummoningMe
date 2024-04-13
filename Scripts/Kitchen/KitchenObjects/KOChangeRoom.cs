using Godot;
using System;

public partial class KOChangeRoom : AKitchenObject
{
    [Export] private int direction;
    [Export] private KitchenRoomController kitchenRoomController;
    [Export] private float wobbleStrength = 0;
    [Export] private float wobbleSpeed = 0.5f;

    private Vector2 basePos;

    public override void _Ready()
    {
        base._Ready();
        basePos = Position;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        float percent = (Mathf.Sin((Time.GetTicksMsec() / 1000f) * wobbleSpeed * Mathf.Pi * direction) + 1) / 2;
        Position = basePos + new Vector2(wobbleStrength * (Easing.EaseInOutQuad(percent) - 0.5f) * 2, 0);
    }

    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    public override void Interact(Hand hand)
    {
        kitchenRoomController.MoveRoom(direction);
    }
}
