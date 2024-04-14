using Godot;
using Godot.Collections;
using System;

public partial class SceneController : Node
{
    private enum State { Idle, FadeOut, FadeIn }
    public static SceneController Current;

    [Export]
    private float SkullRotateTimes;
    [Export]
    private string FirstScene;
    [Export]
    private Dictionary<string, PackedScene> Scenes;
    [Export]
    private Timer Timer;
    [Export]
    private Control SkullSplash;
    [Export]
    private Node ScenesNode;

    private State state;
    private Node currentScene = null;
    private Action midTransition;
    private Action postTransition;

    public override void _Ready()
    {
        base._Ready();
        SkullSplash.PivotOffset = SkullSplash.Size / 2;
        Current = this;
        TransitionToScene(FirstScene);
        //FinishFadeOut();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        switch (state)
        {
            case State.Idle:
                break;
            case State.FadeOut:
                SkullSplash.Scale = Vector2.One * (Easing.EaseOutSin(Timer.Percent()) + 0.0001f);
                SkullSplash.Rotation = SkullRotateTimes * Easing.EaseOutSin(Timer.Percent()) * Mathf.Pi * 2;
                if (Timer.TimeLeft <= 0)
                {
                    FinishFadeOut();
                }
                break;
            case State.FadeIn:
                SkullSplash.Scale = Vector2.One * ((1 - Easing.EaseInSin(Timer.Percent())) + 0.0001f);
                SkullSplash.Rotation = SkullRotateTimes * (1 + Easing.EaseInSin(Timer.Percent())) * Mathf.Pi * 2;
                if (Timer.TimeLeft <= 0)
                {
                    FinishFadeIn();
                }
                break;
            default:
                break;
        }
    }

    private void FinishFadeOut()
    {
        SkullSplash.Scale = Vector2.One;
        state = State.FadeIn;
        midTransition?.Invoke();
        Timer.Start();
    }

    private void FinishFadeIn()
    {
        SkullSplash.Scale = Vector2.One * 0.0001f;
        SkullSplash.Visible = false;
        SkullSplash.MouseFilter = Control.MouseFilterEnum.Ignore;
        state = State.Idle;
        postTransition?.Invoke();
    }

    public void Transition(Action midTransition, Action postTransition)
    {
        SkullSplash.MouseFilter = Control.MouseFilterEnum.Stop;
        SkullSplash.Visible = true;
        this.midTransition = midTransition;
        this.postTransition = postTransition;
        state = State.FadeOut;
        Timer.Start();
    }

    public void TransitionToScene(string name, Action midTransition = null)
    {
        Transition(() =>
        {
            ClearCurrentScene();
            ScenesNode.AddChild(currentScene = Scenes[name].Instantiate<Node>());
            midTransition?.Invoke();
        }, null);
    }

    private void ClearCurrentScene()
    {
        if (currentScene != null)
        {
            currentScene.QueueFree();
            currentScene = null;
        }
    }
}
