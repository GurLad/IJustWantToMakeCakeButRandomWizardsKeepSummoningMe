using Godot;
using System;

public partial class FloatingX : Node
{
    [Export] private Sprite2D display;
    [Export] private float UpSpeed = 20f;
    [Export] private float DisplayTime = 1f;
    [Export] private float FadeTime = 0.2f;
    private Interpolator interpolator = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
    }

    public void Display(Vector2 pos)
    {
        display.Position = pos;
        interpolator.Delay(DisplayTime);
        interpolator.OnFinish = () =>
        {
            interpolator.Interpolate(FadeTime,
                new Interpolator.InterpolateObject(
                    a => display.SelfModulate = new Color(display.SelfModulate, a),
                    1,
                    0),
                new Interpolator.InterpolateObject(
                    a => display.SelfModulate = new Color(display.SelfModulate, a),
                    1,
                    0));
            interpolator.OnFinish = () => QueueFree();
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        display.Position += Vector2.Up * UpSpeed * (float)delta;
    }
}
