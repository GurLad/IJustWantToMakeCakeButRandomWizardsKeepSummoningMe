using Godot;
using System;
using System.Collections.Generic;

public partial class SchoolEquationsController : ASchoolSubActivityController
{
    [Export] private PackedScene sceneEquation;
    [Export] private VBoxContainer[] containers;
    [Export] private int count;

    private List<SchoolEquation> equations = new List<SchoolEquation>();

    public override void _Ready()
    {
        base._Ready();
        for (int i = 0; i < count; i++)
        {
            foreach (var container in containers)
            {
                SchoolEquation newEquation = sceneEquation.Instantiate<SchoolEquation>();
                container.AddChild(newEquation);
                newEquation.Init();
                newEquation.Answered += FinishedEquation;
                newEquation.Visible = true;
                equations.Add(newEquation);
            }
        }
    }

    public override bool ShowNext()
    {
        if (equations.Count > 1)
        {
            equations[0].Select();
            equations.RemoveAt(0);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FinishedEquation()
    {
        EmitSignal(SignalName.Answered);
    }
}
