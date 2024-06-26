using Godot;
using System;
using static ExtensionMethods;

public partial class SchoolQuestion : Control
{
    private enum MistakeType { PlusMinus, MultipleOf10, Random, EndMarker }

    [Export] private float arriveTime;
    [Export] private float leaveTime;
    [Export] private Label label;
    [Export] private PackedScene sceneFloatingX;
    [ExportGroup("SFX")]
    [Export] private AudioStream[] askSFX;
    [Export] private AudioStream[] yesSFX;
    [Export] private AudioStream[] noSFX;

    private Interpolator interpolator = new Interpolator();
    private AudioStreamPlayer2D streamPlayer2D = new AudioStreamPlayer2D();
    private bool ready = false;
    private Vector2 baseScale;
    private bool correct;

    [Signal]
    public delegate void AnsweredEventHandler();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
        AddChild(streamPlayer2D);
        streamPlayer2D.Stream = askSFX.RandomItemInList();
        streamPlayer2D.Play();
        baseScale = Scale;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (ready && @event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            if (keyEvent.Keycode == Key.Y || keyEvent.Keycode == Key.N)
            {
                if (correct ? keyEvent.Keycode == Key.Y : keyEvent.Keycode == Key.N)
                {
                    Leave();
                }
                else
                {
                    SchoolController.Mistakes++;
                    FloatingX.Display(sceneFloatingX, label.Position + new Vector2(RNG.Next(-2, 3) + PivotOffset.X, 0), this);
                }
            }
        }
    }

    public void Display()
    {
        PivotOffset = Size - new Vector2(Size.X / 2, 0);
        Position -= PivotOffset;
        GenerateQuestion();
        interpolator.Interpolate(arriveTime, new Interpolator.InterpolateObject(
            a => Scale = baseScale * a,
            0,
            1,
            Easing.EaseOutBack));
        Scale = Vector2.Zero;
        interpolator.OnFinish = () => ready = true;
    }

    private void Leave()
    {
        ready = false;
        streamPlayer2D.Stream = (correct ? yesSFX : noSFX).RandomItemInList();
        streamPlayer2D.Play();
        interpolator.Interpolate(arriveTime, new Interpolator.InterpolateObject(
            a => Scale = baseScale * a,
            1,
            0,
            Easing.EaseInBack));
        interpolator.OnFinish = () =>
        {
            EmitSignal(SignalName.Answered);
            QueueFree();
        };
    }

    private void GenerateQuestion()
    {
        int num1 = RNG.Next(0, 100);
        int num2 = RNG.Next(0, 100 - num1);
        int correctAns = num1 + num2;
        int ans;
        correct = RNG.Next(0, 2) == 0;
        if (correct)
        {
            ans = correctAns;
        }
        else
        {
            MistakeType mistakeType = (MistakeType)RNG.Next(0, (int)MistakeType.EndMarker);
            int mod;
            switch (mistakeType)
            {
                case MistakeType.PlusMinus:
                    mod = RNG.Next(-5, 5);
                    if (mod >= 0) mod++;
                    ans = Mathf.Clamp(correctAns + mod, 0, 99);
                    break;
                case MistakeType.MultipleOf10:
                    mod = RNG.Next(-2, 2);
                    if (mod >= 0) mod++;
                    if (correctAns + mod * 10 > 99) mod *= -1;
                    if (correctAns + mod * 10 < 0) mod *= -1;
                    ans = Mathf.Clamp(correctAns + mod * 10, 0, 99);
                    break;
                case MistakeType.Random:
                    ans = RNG.Next(0, 100);
                    break;
                case MistakeType.EndMarker:
                default:
                    throw new Exception("Impossible");
            }
            if (ans == correctAns)
            {
                ans = correctAns > 0 ? (correctAns - 1) : (correctAns + 1);
            }
        }
        label.Text = num1 + "+" + num2 + "=" + ans;
    }
}
