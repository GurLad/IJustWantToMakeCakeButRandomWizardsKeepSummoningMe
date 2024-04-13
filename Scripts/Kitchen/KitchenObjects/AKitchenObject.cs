using Godot;
using System;

public abstract partial class AKitchenObject : Sprite2D
{
    [Export] protected AudioStream finishSFX;
    [Export] private string tooltip;
    [Export] private float tooltipOffset = 3f;
    [ExportGroup("Internal")]
    [Export] private float tooltipFadeInDelay = 0.1f;
    [Export] private float tooltipFadeTime = 0.2f;
    [Export] private Area2D area;
    [Export] private CollisionShape2D collisionShape;
    [Export] private Tooltip tooltipObject;
    [Export] private Material outlineMaterial;

    private Interpolator interpolator = new Interpolator();
    private AudioStreamPlayer2D streamPlayer = new AudioStreamPlayer2D();

    public abstract bool CanInteract(Hand hand);
    protected abstract void InteractAction(Hand hand);

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
        AddChild(streamPlayer);
        RectangleShape2D trueShape = new RectangleShape2D();
        trueShape.Size = Texture.GetSize();
        collisionShape.Shape = trueShape;
        collisionShape.Position = trueShape.Size / 2f;
        // Tooltip
        tooltipObject.Text = tooltip;
        tooltipObject.Alpha = 0;
        tooltipObject.Position += new Vector2(0, (trueShape.Size.Y + tooltipObject.Size.Y) / 2 + tooltipOffset);
        tooltipObject.Visible = true;
        // Blegh
        area.MouseEntered += () => { if (Hand.Current != null) Hand.Current.EnterKitchenObject(this); };
        area.MouseExited += () => { if (Hand.Current != null) Hand.Current.LeaveKitchenObject(this); };
    }

    public void Enter(Hand hand)
    {
        Material = outlineMaterial;
        if (!string.IsNullOrEmpty(tooltip))
        {
            interpolator.Delay(tooltipFadeInDelay);
            interpolator.Interpolate(tooltipFadeTime, new Interpolator.InterpolateObject(
                a => tooltipObject.Alpha = a,
                tooltipObject.Alpha,
                1,
                Easing.EaseInOutSin));
        }
    }

    public void Leave(Hand hand)
    {
        Material = null;
        if (!string.IsNullOrEmpty(tooltip))
        {
            interpolator.Interpolate(tooltipFadeTime, new Interpolator.InterpolateObject(
            a => tooltipObject.Alpha = a,
            tooltipObject.Alpha,
            0,
            Easing.EaseInOutSin));
        }
    }

    public virtual void Interact(Hand hand)
    {
        Play(finishSFX);
        InteractAction(hand);
    }

    protected void Play(AudioStream stream)
    {
        streamPlayer.Stop();
        if (stream != null)
        {
            streamPlayer.Stream = stream;
            streamPlayer.Play();
        }
    }
}
