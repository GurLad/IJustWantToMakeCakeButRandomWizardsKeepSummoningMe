using Godot;
using System;
using System.Collections.Generic;

public static class IngredientController
{
    private static List<IngredientData> datas;

    public static void LoadData()
    {

    }

    public static Ingredient Get(string name)
    {
        return new Ingredient(datas.Find(a => a.Name == name));
    }
}
