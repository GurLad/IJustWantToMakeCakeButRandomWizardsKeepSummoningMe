using Godot;
using System;
using System.Collections.Generic;

public partial class KOBowl : ATimedKitchenObject
{
    public List<Ingredient> Ingredients { private get; set; } = new List<Ingredient>();
    public bool IsEmpty => Ingredients.Count <= 0;

    [Export] private KitchenController kitchenController;

    public override void _Ready()
    {
        base._Ready();
        kitchenController?.ConnectBowl(this);
    }

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty;
    }

    protected override void InteractAction(Hand hand)
    {
        List<Ingredient> newIngredients = hand.TakeAllIngredients();
        Ingredient clone = null;
        foreach (var ingredient in newIngredients)
        {
            if ((clone = Ingredients.Find(a => a.Name == ingredient.Name && a.State == ingredient.State)) != null)
            {
                clone.Count += ingredient.Count;
            }
            else
            {
                Ingredients.Add(ingredient);
            }
        }
    }

    public List<Ingredient> TakeAllIngredients()
    {
        List<Ingredient> result = new List<Ingredient>(Ingredients);
        Ingredients.Clear();
        return result;
    }
}
