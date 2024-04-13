using Godot;
using System;

public partial class FadeTransition : ColorRect
{
    [Export] private float transitionTime;

    private Interpolator interpolator = new Interpolator();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
        Modulate = new Color(Modulate, 0);
        Visible = false;
    }

    public void Transition(Action midTransition, Action postTransition, Func<float, float> easing = null)
    {
        Visible = true;
        MouseFilter = MouseFilterEnum.Stop;
        interpolator.Interpolate(transitionTime, new Interpolator.InterpolateObject(
            a => Modulate = new Color(Modulate, a),
            0,
            1,
            easing));
        interpolator.OnFinish = () =>
        {
            midTransition?.Invoke();
            interpolator.Interpolate(transitionTime, new Interpolator.InterpolateObject(
                a => Modulate = new Color(Modulate, a),
                1,
                0,
                easing));
            interpolator.OnFinish = () =>
            {
                MouseFilter = MouseFilterEnum.Ignore;
                Visible = false;
                postTransition?.Invoke();
            };
        };
    }
}
