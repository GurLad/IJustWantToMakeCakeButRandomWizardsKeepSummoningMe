using Godot;
using System;
using System.Collections.Generic;

public partial class Hand : Node
{
    [Export] private int maxDifferentIngredients = 5;
    [Export] private HandCursor cursor;

    private List<Ingredient> ingredients;
    private Interpolator interpolator = new Interpolator();

    public bool IsFull => ingredients.Count >= maxDifferentIngredients;
    public bool IsEmpty => ingredients.Count <= 0;

    [Signal]
    public delegate void BeganTimedActionEventHandler();

    [Signal]
    public delegate void FinishedTimedActionEventHandler();

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
    }

    public void AddIngredient(string name)
    {
        Ingredient target = ingredients.Find(a => a.Name == name);
        if (target != null)
        {
            target.Count++;
        }
        else
        {
            target = IngredientController.Get(name);
            ingredients.Add(target);
        }
        VisualiseIngredients();
    }

    public bool CanApplyState(IngredientState target)
    {
        return ingredients.FindIndex(a => !a.CanApplyState(target)) < 0;
    }

    public void ApplyState(IngredientState target)
    {
        ingredients.ForEach(a => a.ApplyState(target));
        VisualiseIngredients();
    }

    public List<Ingredient> TakeAllIngredients()
    {
        List<Ingredient> result = new List<Ingredient>(ingredients);
        ingredients.Clear();
        VisualiseIngredients();
        return result;
    }

    public void BeginTimedAction(float time, Action onFinish)
    {
        EmitSignal(SignalName.BeganTimedAction);
        cursor.SetTimed();
        interpolator.Interpolate(time,
            new Interpolator.InterpolateObject(
                a => cursor.TimedPercent = a,
                0,
                1));
        interpolator.OnFinish = () => FinishTimedAction(onFinish);
    }

    private void FinishTimedAction(Action onFinish)
    {
        cursor.SetNormal();
        onFinish?.Invoke();
        EmitSignal(SignalName.FinishedTimedAction);
    }

    private void VisualiseIngredients()
    {
        // TBA
    }
}
