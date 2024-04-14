using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EndScreen : Node
{
    // Wow, so much hardcoding~
    private static Dictionary<IngredientState, string[]> STATE_TITLES { get; } = new Dictionary<IngredientState, string[]>()
    {
        { IngredientState.Normal, new string[] { "Astonishingly Basic", "Definitely Normal", "Low Effort" } },
        { IngredientState.Burned, new string[] { "Flaming Hot", "Crispy Terror", "Jaw's Bane" } },
        { IngredientState.Cut, new string[] { "Tasty Mush", "Salad Lover's", "Serial Murderer's", "Very Edgy" } },
        { IngredientState.Burned | IngredientState.Cut, new string[] { "Summoner's Beloved", "Busy Bee's", "High Maintenance" } }
    };
    private static string[] CAKE_TITLES { get; } = new string[] { "Delight", "Wonder", "Dream", "Cake", "Epiphany" };
    private static List<List<Ingredient>> PERFECT_STIRRED_PARTS { get; } = new List<List<Ingredient>>()
    {
        new List<Ingredient>()
        {
            new Ingredient("LizardTail", IngredientState.Cut, 5),
            new Ingredient("Eyeball", IngredientState.Normal, 6)
        },
        new List<Ingredient>()
        {
            new Ingredient("Tentacle", IngredientState.Burned, 1),
            new Ingredient("FunGuys", IngredientState.Cut, 4)
        },
        new List<Ingredient>()
        {
            new Ingredient("GrumpyFlower", IngredientState.Normal, 15),
            new Ingredient("MrFluffy", IngredientState.Burned, 1)
        }
    };
    private static List<Ingredient> PERFECT_BOWL_PARTS = new List<Ingredient>() { new Ingredient("Tentacle", IngredientState.Normal, 1) };

    [Export] private Godot.Collections.Dictionary<string, Texture2D> cakeTextures;
    [Export] private Label recipeLabel;
    [Export] private Label[] cakeNameLabels;
    [Export] private Sprite2D cakeSprite;
    [Export] private Label summingMistakesLabel;
    [Export] private Label scoreLabel;

    public override void _Ready()
    {
        base._Ready();
        KitchenController.KitchenSaveState kitchenState = KitchenController.GetFinalSaveState();
        string[] cakeNameParts = GenerateCakeNameParts(kitchenState);
        float scorePercent = GetScorePercent(kitchenState);
        if (scorePercent >= 1 - Mathf.Epsilon)
        {
            cakeNameParts[1] = "Perfect";
        }
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
        summingMistakesLabel.Text = string.Format(summingMistakesLabel.Text, SchoolController.Mistakes);
        scoreLabel.Text = string.Format(scoreLabel.Text, Mathf.RoundToInt(scorePercent * 100));
        SchoolController.Mistakes = 0;
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
        finalState |= stateCount[IngredientState.Burned] > stateCount[IngredientState.Normal] / 3 ? IngredientState.Burned : IngredientState.Normal;
        finalState |= stateCount[IngredientState.Cut] > stateCount[IngredientState.Normal] / 3 ? IngredientState.Cut : IngredientState.Normal;
        //finalState |= stateCount[IngredientState.Wet] > stateCount[IngredientState.Normal] ? IngredientState.Wet : IngredientState.Normal;
        List<string> sortedKeys = typesCount.Keys.ToList();
        sortedKeys.Sort((a, b) => -typesCount[a].CompareTo(typesCount[b]));
        result[0] = "The " + STATE_TITLES[finalState].RandomItemInList();
        result[1] = sortedKeys.Count > 0 ? sortedKeys[0].FixName() : "Nothing";
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
                AddLine("Put " + ingredient.ToString() + " in a bowl.");
            }
            AddLine(i == 0 ? "Stir" : (i == 1 ? "Stir again" : "Stir yet again"));
        }
        foreach (Ingredient ingredient in kitchenState.BowlContents)
        {
            AddLine("Put " + ingredient.ToString() + " in a bowl.");
        }
        AddLine("Put everything in the oven.");
        AddLine("Bake for 30 minutes.");
        AddLine("Enjoy your " + cakeName + "!");
        return result.Trim();
    }

    private float GetScorePercent(KitchenController.KitchenSaveState kitchenState)
    {
        int result = 0;
        int max = 0;
        for (int i = 0; i < PERFECT_STIRRED_PARTS.Count; i++)
        {
            max += 3 * PERFECT_STIRRED_PARTS[i].Count;
            if (i < kitchenState.CakeParts.Count)
            {
                result += ScoreGroup(PERFECT_STIRRED_PARTS[i], kitchenState.CakeParts[i]);
            }
        }
        max += 3 * PERFECT_BOWL_PARTS.Count;
        result += ScoreGroup(PERFECT_BOWL_PARTS, kitchenState.BowlContents);
        GD.Print("Result: " + result + " / " + max);
        return (result + 0.0001f) / max;
    }

    private int ScoreGroup(List<Ingredient> correct, List<Ingredient> input)
    {
        int currentResult = 0;
        foreach (var item in correct)
        {
            Ingredient match = input.Find(a => a.Name == item.Name);
            if (match != null)
            {
                GD.Print("+" + ScoreMatch(item, match) + ": " + item + " has match: " + match);
                currentResult += ScoreMatch(item, match);
            }
            else
            {
                GD.Print("+0: " + item + " has no match");
            }
        }
        foreach (var item in input.FindAll(a => correct.Find(b => b.Name == a.Name) == null))
        {
            GD.Print("-1: " + item + " is in input but not in correct");
            currentResult--;
        }
        return Mathf.Max(0, currentResult);
    }

    private int ScoreMatch(Ingredient item, Ingredient match)
    {
        return 1 + (match.Count == item.Count ? 1 : 0) + (match.State == item.State ? 1 : 0);
    }
}
