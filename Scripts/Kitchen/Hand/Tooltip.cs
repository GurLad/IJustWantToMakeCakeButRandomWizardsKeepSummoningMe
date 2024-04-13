using Godot;
using System;

public partial class Tooltip : Control
{
    [Export] private Label label;

    public string Text
    {
        get => label.Text;
        set => label.Text = value;
    }

    public float Alpha
    {
        get => Modulate.A;
        set => Modulate = new Color(Modulate, value);
    }
}
