using Godot;
using System;

public partial class HandCursor : Control
{
    [Export] private TextureRect normal;
    [Export] private TextureProgressBar working;

    public float TimedPercent
    {
        set => working.Value = value;
    }

    public void SetTimed()
    {
        normal.Visible = false;
        working.Visible = true;
    }
    
    public void SetNormal()
    {
        normal.Visible = true;
        working.Visible = false;
    }
}
