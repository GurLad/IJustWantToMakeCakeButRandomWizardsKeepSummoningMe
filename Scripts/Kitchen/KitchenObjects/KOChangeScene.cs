using Godot;
using System;

public partial class KOChangeScene : AKitchenObject
{
    [Export] private string sceneName;

    public override bool CanInteract(Hand hand)
    {
        return true;
    }

    protected override void InteractAction(Hand hand)
    {
        SceneController.Current.TransitionToScene(sceneName);
    }
}
