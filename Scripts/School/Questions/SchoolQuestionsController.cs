using Godot;
using System;

public partial class SchoolQuestionsController : ASchoolSubActivityController
{
    public override bool ShowNext()
    {
        EmitSignal(SignalName.Answered);
        return true;
    }
}
