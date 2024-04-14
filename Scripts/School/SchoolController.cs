using Godot;
using System;
using static ExtensionMethods;

public partial class SchoolController : Node
{
    [Export] private ASchoolSubActivityController equationsController;
    [Export] private ASchoolSubActivityController questionsController;
    [Export] private float questionsRate = 0.3f;

    public override void _Ready()
    {
        base._Ready();
        equationsController.Background.Visible = questionsController.Background.Visible = false;
        equationsController.Answered += NextActivity;
        questionsController.Answered += NextActivity;
    }

    public void NextActivity()
    {
        equationsController.Background.Visible = questionsController.Background.Visible = false;
        ASchoolSubActivityController activity = RNG.NextFloat(0, 1) <= questionsRate ? questionsController : equationsController;
        activity.Background.Visible = true;
        if (!activity.ShowNext())
        {
            // TBA: Return to kitchen
        }
    }
}
