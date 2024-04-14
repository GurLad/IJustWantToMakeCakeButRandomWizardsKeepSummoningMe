using Godot;
using System;

public partial class GlowingQuestionMark : Label
{
    [Export] private float minOpacity;
    [Export] private float rate;

    public override void _Process(double delta)
    {
        base._Process(delta);
        float percent = (Mathf.Sin((Time.GetTicksMsec() / 1000f) * rate * Mathf.Pi) + 1) / 2;
        SelfModulate = new Color(SelfModulate, percent * (1 - minOpacity) + minOpacity);
    }
}
