using Godot;
using System;

public partial class SchoolQuestionsController : ASchoolSubActivityController
{
    [Export] private PackedScene sceneQuestion;
    [Export] private Control[] spawnPoints;

    public override bool ShowNext()
    {
        SchoolQuestion schoolQuestion = sceneQuestion.Instantiate<SchoolQuestion>();
        AddChild(schoolQuestion);
        schoolQuestion.Answered += () => EmitSignal(SignalName.Answered);
        schoolQuestion.Position = spawnPoints.RandomItemInList().Position;
        schoolQuestion.Display();
        return true;
    }
}
