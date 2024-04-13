using Godot;
using System;
using System.Collections.Generic;

public static class IngredientController
{
    private static List<IngredientData> datas = new List<IngredientData>();

    public static void LoadData()
    {

    }

    public static Ingredient Get(string name)
    {
        return new Ingredient(datas.Find(a => a.Name == name) ?? throw new Exception("Missing ingredient! " + name));
    }
}
