using Godot;
using System;
using System.Collections.Generic;

public partial class SchoolEquationsController : Node
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
        // TEMP
        equations[0].Select();
    }

    private void FinishedEquation()
    {
        // TEMP
        equations.RemoveAt(0);
        equations[0].Select();
    }
}
