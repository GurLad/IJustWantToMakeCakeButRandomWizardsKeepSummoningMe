using Godot;
using System;
using System.Collections.Generic;
using static ExtensionMethods;

public partial class KitchenController : Node
{
    private static KitchenSaveState saveState = new KitchenSaveState();

    [Export] private Vector2 TeleportRate;

    private Timer timer = new Timer();
    private bool readyToTeleport = false;
    private bool middleOfAction = false;

    public override void _Ready()
    {
        base._Ready();
        AddChild(timer);
        if (!saveState.FirstTime)
        {
            timer.WaitTime = RNG.NextFloat(TeleportRate);
            timer.Timeout += TryToTeleport;
            timer.Start();
        }
        Hand.Current.BeganTimedAction += BeginTimedAction;
        Hand.Current.FinishedTimedAction += FinishTimedAction;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Hand.Current.BeganTimedAction -= BeginTimedAction;
        Hand.Current.FinishedTimedAction -= FinishTimedAction;
    }

    public void AddCakeIngredients(List<Ingredient> ingredients)
    {
        saveState.CakeParts.Add(new List<Ingredient>(ingredients));
    }

    private void BeginTimedAction()
    {
        middleOfAction = true;
    }

    private void FinishTimedAction()
    {
        middleOfAction = false;
        if (readyToTeleport || saveState.FirstTime)
        {
            Teleport();
        }
    }

    private void TryToTeleport()
    {
        if (middleOfAction)
        {
            readyToTeleport = true;
        }
        else
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        saveState.FirstTime = false;
        MusicController.Play("MathEqualsTension");
        SceneController.Current.TransitionToScene("School");
    }

    private class KitchenSaveState
    {
        public bool FirstTime = true;
        public List<List<Ingredient>> CakeParts = new List<List<Ingredient>>();
    }
}
