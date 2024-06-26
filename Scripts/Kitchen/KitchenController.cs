using Godot;
using System;
using System.Collections.Generic;
using static ExtensionMethods;

public partial class KitchenController : Node
{
    private static KitchenSaveState saveState = new KitchenSaveState();

    [Export] private Vector2 TeleportRate;
    [Export] private KOOven oven;
    [Export] private KitchenRoomController kitchenRoomController;

    private Timer timer = new Timer();
    private bool readyToTeleport = false;
    private bool middleOfAction = false;

    public static KitchenSaveState GetFinalSaveState()
    {
        KitchenSaveState temp = saveState;
        saveState = new KitchenSaveState();
        return temp;
    }

    public override void _Ready()
    {
        base._Ready();
        AddChild(timer);
        if (!saveState.FirstTime)
        {
            kitchenRoomController.SetRoom(saveState.CurrentRoom);
            timer.WaitTime = RNG.NextFloat(TeleportRate);
            timer.Timeout += TryToTeleport;
            timer.Start();
        }
        else
        {
            kitchenRoomController.ToggleRecipeRoom(true, false);
            oven.Visible = false;
        }
        Hand.Current.BeganTimedAction += BeginTimedAction;
        Hand.Current.FinishedTimedAction += FinishTimedAction;
        Hand.Current.CancledTimedAction += FinishTimedAction;
        GD.Print(saveState);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Hand.Current.BeganTimedAction -= BeginTimedAction;
        Hand.Current.FinishedTimedAction -= FinishTimedAction;
        Hand.Current.CancledTimedAction -= FinishTimedAction;
    }

    public void AddCakeIngredients(List<Ingredient> ingredients)
    {
        saveState.CakeParts.Add(new List<Ingredient>(ingredients));
    }

    public void ConnectBowl(KOBowl bowl)
    {
        bowl.Ingredients = saveState.BowlContents;
    }

    public void Kill()
    {
        // Overkill
        middleOfAction = true;
        timer.Stop();
        readyToTeleport = false;
        QueueFree();
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
        saveState.CurrentRoom = kitchenRoomController.CurrentRoom;
        MusicController.Play("MathEqualsTension");
        SceneController.Current.TransitionToScene("School");
    }

    public class KitchenSaveState
    {
        public bool FirstTime = true;
        public List<Ingredient> BowlContents = new List<Ingredient>();
        public List<List<Ingredient>> CakeParts = new List<List<Ingredient>>();
        public int CurrentRoom = 1;

        public override string ToString()
        {
            return "First time: " + FirstTime + "\nBowl:\n" + BowlContents.PrettyPrint() +
                "\nCake:\n" + CakeParts.PrettyPrint();
        }
    }
}
