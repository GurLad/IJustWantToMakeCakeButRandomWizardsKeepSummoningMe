using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EndScreen : Node
{
    private static Dictionary<IngredientState, string[]> STATE_TITLES { get; } = new Dictionary<IngredientState, string[]>()
    {
        { IngredientState.Normal, new string[] { "Astonishingly Basic", "Definitely Normal", "Low Effort" } },
        { IngredientState.Burned, new string[] { "Flaming Hot", "Crispy Terror", "Jaw's Bane" } },
        { IngredientState.Cut, new string[] { "Tasty Mush", "Salad Lover's", "Serial Murderer's" } },
        { IngredientState.Burned | IngredientState.Cut, new string[] { "Summoner's Beloved", "Busy Bee's", "High Maintenance" } }
    };
    private static string[] CAKE_TITLES { get; } = new string[] { "Delight", "Wonder", "Dream", "Cake", "Epiphany" };

    [Export] private Godot.Collections.Dictionary<string, Texture2D> cakeTextures;
    [Export] private Label recipeLabel;
    [Export] private Label[] cakeNameLabels;
    [Export] private Sprite2D cakeSprite;

    public override void _Ready()
    {
        base._Ready();
        KitchenController.KitchenSaveState kitchenState = KitchenController.GetFinalSaveState();
        string[] cakeNameParts = GenerateCakeNameParts(kitchenState);
        for (int i = 0; i < Mathf.Min(cakeNameLabels.Length, cakeNameParts.Length); i++)
        {
            cakeNameLabels[i].Text = cakeNameParts[i];
        }
        recipeLabel.Text = GenerateRecipe(kitchenState, string.Join(" ", cakeNameParts).Substring(4));
        if (cakeTextures.ContainsKey(cakeNameParts[1]))
        {
            cakeSprite.Texture = cakeTextures[cakeNameParts[1]];
        }
        else
        {
            GD.PrintErr("Cake not found! " + cakeNameParts[1]);
        }
    }

    private string[] GenerateCakeNameParts(KitchenController.KitchenSaveState kitchenState)
    {
        int total = 0;
        string[] result = new string[3];
        Dictionary<IngredientState, int> stateCount = new Dictionary<IngredientState, int>();
        stateCount.Add(IngredientState.Normal, 0);
        stateCount.Add(IngredientState.Burned, 0);
        stateCount.Add(IngredientState.Cut, 0);
        stateCount.Add(IngredientState.Wet, 0);
        Dictionary<string, int> typesCount = new Dictionary<string, int>();
        foreach (Ingredient ingredient in kitchenState.BowlContents)
        {
            total += ingredient.Count;
            stateCount[ingredient.State] += ingredient.Count;
            if (!typesCount.ContainsKey(ingredient.Name))
            {
                typesCount.Add(ingredient.Name, ingredient.Count);
            }
            else
            {
                typesCount[ingredient.Name] += ingredient.Count;
            }
        }
        for (int i = 0; i < kitchenState.CakeParts.Count; i++)
        {
            List<Ingredient> stirBlock = kitchenState.CakeParts[i];
            foreach (Ingredient ingredient in stirBlock)
            {
                total += ingredient.Count;
                stateCount[ingredient.State] += ingredient.Count;
                if (!typesCount.ContainsKey(ingredient.Name))
                {
                    typesCount.Add(ingredient.Name, ingredient.Count);
                }
                else
                {
                    typesCount[ingredient.Name] += ingredient.Count;
                }
            }
        }
        IngredientState finalState = IngredientState.Normal;
        finalState |= stateCount[IngredientState.Burned] > stateCount[IngredientState.Normal] ? IngredientState.Burned : IngredientState.Normal;
        finalState |= stateCount[IngredientState.Cut] > stateCount[IngredientState.Normal] ? IngredientState.Cut : IngredientState.Normal;
        //finalState |= stateCount[IngredientState.Wet] > stateCount[IngredientState.Normal] ? IngredientState.Wet : IngredientState.Normal;
        List<string> sortedKeys = typesCount.Keys.ToList();
        sortedKeys.Sort((a, b) => -typesCount[a].CompareTo(typesCount[b]));
        result[0] = "The " + STATE_TITLES[finalState].RandomItemInList();
        result[1] = sortedKeys.Count > 0 ? FixName(sortedKeys[0]) : "Nothing";
        result[2] = CAKE_TITLES.RandomItemInList();
        return result;
    }

    private string GenerateCakeName(KitchenController.KitchenSaveState kitchenState)
    {
        return string.Join(" ", GenerateCakeNameParts(kitchenState));
    }

    private string GenerateRecipe(KitchenController.KitchenSaveState kitchenState, string cakeName)
    {
        string result = "";
        int lineNumber = 1;

        void AddLine(string line) => result += (lineNumber++) + ". " + line + "\n";

        for (int i = 0; i < kitchenState.CakeParts.Count; i++)
        {
            List<Ingredient> stirBlock = kitchenState.CakeParts[i];
            foreach (Ingredient ingredient in stirBlock)
            {
                AddLine("Put " + FixName(ingredient.ToString()) + " in a bowl.");
            }
            AddLine(i == 0 ? "Stir" : (i == 1 ? "Stir again" : "Stir yet again"));
        }
        foreach (Ingredient ingredient in kitchenState.BowlContents)
        {
            AddLine("Put " + FixName(ingredient.ToString()) + " in a bowl.");
        }
        AddLine("Put everything in the oven.");
        AddLine("Bake for 30 minutes.");
        AddLine("Enjoy your " + cakeName + "!");
        return result.Trim();
    }

    private string FixName(string name)
    {
        return name switch
        {
            "MrFluffy" => "Mr. Fluffy",
            "GrumpyFlower" => "Grumpy Flower",
            "EnergyDrink" => "Energy Drink",
            "LizardTail" => "Lizard",
            "FunGuys" => "Fun Guys",
            _ => name
        };
    }
}
