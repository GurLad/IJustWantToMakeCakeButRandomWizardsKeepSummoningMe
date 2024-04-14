using Godot;
using System;

public partial class GlowingQuestionMark : Label
{
    [Export] private float minOpacity;
    [Export] private float rate;

    public override void _Process(double delta)
    {
        base._Process(delta);
        float percent = ExtensionMethods.SinTime(rate);
        SelfModulate = new Color(SelfModulate, percent * (1 - minOpacity) + minOpacity);
    }
}
