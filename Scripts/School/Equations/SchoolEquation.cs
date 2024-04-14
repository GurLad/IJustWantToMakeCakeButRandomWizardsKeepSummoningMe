using Godot;
using System;
using static ExtensionMethods;

public partial class SchoolEquation : Label
{
    [Export] private GlowingQuestionMark questionMark;
    [Export] private PackedScene sceneFloatingX;
    [Export] private AudioStream correctAudioStream;

    private int answer;
    private bool selected = false;
    private int delayedSelect = -1;
    private AudioStreamPlayer2D streamPlayer2D = new AudioStreamPlayer2D();

    [Signal]
    public delegate void AnsweredEventHandler();

    public override void _Ready()
    {
        base._Ready();
        AddChild(streamPlayer2D);
    }

    public void Init()
    {
        int num1 = RNG.Next(0, 10);
        int num2 = RNG.Next(0, 10 - num1);
        answer = num1 + num2;
        Text = num1 + "+" + num2;
        questionMark.Visible = false;
    }

    public void Select()
    {
        Text += "=";
        questionMark.Visible = true;
        delayedSelect = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // Bad code weeeee
        if (delayedSelect >= 0)
        {
            delayedSelect++;
            if (delayedSelect >= 2)
            {
                delayedSelect = -1;
                selected = true;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (selected && @event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            if (keyEvent.KeyLabel >= Key.Key0 && keyEvent.KeyLabel <= Key.Key9)
            {
                if (keyEvent.KeyLabel - Key.Key0 == answer)
                {
                    questionMark.Visible = false;
                    Text += answer;
                    delayedSelect = -1;
                    selected = false;
                    streamPlayer2D.Stream = correctAudioStream;
                    streamPlayer2D.Play();
                    EmitSignal(SignalName.Answered);
                }
                else
                {
                    SchoolController.Mistakes++;
                    FloatingX.Display(sceneFloatingX, questionMark.Position + new Vector2(RNG.Next(-2, 3), 0), this);
                }
            }
        }
    }
}
