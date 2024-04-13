using Godot;
using System;
using System.Collections.Generic;

public partial class Hand : Control
{
    public static Hand Current { get; private set; }

    [Export] private int maxDifferentIngredients = 5;
    [Export] private HandCursor cursor;
    [Export] private IngredientsDisplay ingredientsDisplay;
    [Export] private PackedScene sceneFloatingX;

    private List<Ingredient> ingredients = new List<Ingredient>();
    private Interpolator interpolator = new Interpolator();
    private AKitchenObject currentKitchenObject;
    private ATimedKitchenObject currentWorkingObject;

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
        Current = this;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left && !mouseButtonEvent.IsEcho())
            {
                if (mouseButtonEvent.Pressed)
                {
                    if (currentKitchenObject?.CanInteract(this) ?? false)
                    {
                        currentKitchenObject?.Interact(this);
                    }
                    else if (currentKitchenObject != null)
                    {
                        FloatingX floatingX = sceneFloatingX.Instantiate<FloatingX>();
                        GetParent().AddChild(floatingX);
                        floatingX.Display(Position);
                    }
                }
                else if (interpolator.Active)
                {
                    interpolator.Stop(false);
                    cursor.SetNormal();
                    currentWorkingObject?.StopAction();
                }
            }
        }
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            Position = mouseMotionEvent.Position;
        }
    }

    public void EnterKitchenObject(AKitchenObject kitchenObject)
    {
        if (currentKitchenObject != null)
        {
            currentKitchenObject.Leave(this);
        }
        currentKitchenObject = kitchenObject;
        currentKitchenObject.Enter(this);
    }

    public void LeaveKitchenObject(AKitchenObject kitchenObject)
    {
        if (currentKitchenObject == kitchenObject)
        {
            currentKitchenObject.Leave(this);
            currentKitchenObject = null;
        }
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

    public bool HasIngredient(string name) => ingredients.Find(a => a.Name == name) != null;

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

    public void BeginTimedAction(ATimedKitchenObject caller, float time, Action onFinish)
    {
        EmitSignal(SignalName.BeganTimedAction);
        currentWorkingObject = caller;
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
        currentWorkingObject = null;
        cursor.SetNormal();
        onFinish?.Invoke();
        EmitSignal(SignalName.FinishedTimedAction);
    }

    private void VisualiseIngredients()
    {
        ingredientsDisplay.Regenerate(ingredients);
    }
}
