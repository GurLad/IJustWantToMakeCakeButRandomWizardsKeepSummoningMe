using Godot;
using System;

public partial class UpDownAnimation : Control
{
    [Export] private float rate;
    [Export] private float strength;

    private Vector2 basePos;

    public override void _Ready()
    {
        base._Ready();
        basePos = Position;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Position = basePos + new Vector2(ExtensionMethods.SinTime(rate) * strength, 0);
    }
}
