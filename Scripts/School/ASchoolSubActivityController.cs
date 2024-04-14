using Godot;
using System;

public abstract partial class ASchoolSubActivityController : Node
{
    [Export] public Sprite2D Background { get; private set; }

    [Signal]
    public delegate void AnsweredEventHandler();

    public abstract bool ShowNext();
}
