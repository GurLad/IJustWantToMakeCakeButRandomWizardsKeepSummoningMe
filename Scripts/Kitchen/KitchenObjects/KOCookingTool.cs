using Godot;
using System;

public partial class KOCookingTool : ATimedKitchenObject
{
    [Export] private IngredientState state;

    public override bool CanInteract(Hand hand)
    {
        return !hand.IsEmpty && hand.CanApplyState(state);
    }

    protected override void InteractAction(Hand hand)
    {
        hand.ApplyState(state);
    }
}
