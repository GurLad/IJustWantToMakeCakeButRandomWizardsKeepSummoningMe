using Godot;
using System;
using System.Collections.Generic;

public partial class EndScreen : Node
{
    private static Dictionary<IngredientState, string[]> STATE_TITLES { get; } = new Dictionary<IngredientState, string[]>()
    {
        { IngredientState.Normal, new string[] { "Astonishingly Basic", "Definitely Normal", "Undercooked & Undercut" } },
        { IngredientState.Burned, new string[] { "Flaming Hot", "Crispy Bones", "Jaw's Bane" } },
        { IngredientState.Cut, new string[] { "Tasty Mush", "Salad Lover's", "Serial Murderer's" } },
        { IngredientState.Burned | IngredientState.Cut, new string[] { "Summoner's Beloved", "Busy Bee's", "High Maintenance" } }
    };
    private static string[] CAKE_TITLES { get; } = new string[] { "Delight", "Wonder", "Dream", "Cake", "Epiphany" };

    [Export] private Label recipeLabel;
    [Export] private Label cakeNameLabel;

    public override void _Ready()
    {
        base._Ready();
        KitchenController.KitchenSaveState kitchenState = KitchenController.GetFinalSaveState();
        recipeLabel.Text = GenerateRecipe(kitchenState, GenerateCakeName(kitchenState, false));
        cakeNameLabel.Text = GenerateCakeName(kitchenState, false);
    }

    private string GenerateCakeName(KitchenController.KitchenSaveState kitchenState, bool addNewlines)
    {
        int total = 0;
        string result = "The" + (addNewlines ? "\n" : " ");
        Dictionary<IngredientState, int> stateCount = new Dictionary<IngredientState, int>();
        stateCount.Add(IngredientState.Normal, 0);
        stateCount.Add(IngredientState.Burned, 0);
        stateCount.Add(IngredientState.Cut, 0);
        stateCount.Add(IngredientState.Wet, 0);
        foreach (Ingredient ingredient in kitchenState.BowlContents)
        {
            total += ingredient.Count;
            stateCount[ingredient.State] += ingredient.Count;
        }
        for (int i = 0; i < kitchenState.CakeParts.Count; i++)
        {
            List<Ingredient> stirBlock = kitchenState.CakeParts[i];
            foreach (Ingredient ingredient in stirBlock)
            {
                total += ingredient.Count;
                stateCount[ingredient.State] += ingredient.Count;
            }
        }
        IngredientState finalState = IngredientState.Normal;
        finalState |= stateCount[IngredientState.Burned] > stateCount[IngredientState.Normal] ? IngredientState.Burned : IngredientState.Normal;
        finalState |= stateCount[IngredientState.Cut] > stateCount[IngredientState.Normal] ? IngredientState.Cut : IngredientState.Normal;
        //finalState |= stateCount[IngredientState.Wet] > stateCount[IngredientState.Normal] ? IngredientState.Wet : IngredientState.Normal;
        result += STATE_TITLES[finalState].RandomItemInList() + (addNewlines ? "\n" : " ");
        result += CAKE_TITLES.RandomItemInList();
        return result;
    }

    private string GenerateRecipe(KitchenController.KitchenSaveState kitchenState, string cakeName)
    {
        string result = "";
        int lineNumber = 1;

        void AddLine(string line) => result += lineNumber + ". " + line + "\n";

        for (int i = 0; i < kitchenState.CakeParts.Count; i++)
        {
            List<Ingredient> stirBlock = kitchenState.CakeParts[i];
            foreach (Ingredient ingredient in stirBlock)
            {
                AddLine("Put " + ingredient.ToString() + " in a bowl");
            }
            AddLine(i == 0 ? "Stir" : (i == 1 ? "Stir again" : "Stir yet again"));
        }
        foreach (Ingredient ingredient in kitchenState.BowlContents)
        {
            AddLine("Put " + ingredient.ToString() + " in a bowl");
        }
        AddLine("Put everything in the oven.");
        AddLine("Bake for 30 minutes.");
        AddLine("Enjoy your " + cakeName + "!");
        return result.Trim();
    }
}
