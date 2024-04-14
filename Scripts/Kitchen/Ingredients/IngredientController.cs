using Godot;
using System;
using System.Collections.Generic;

public static class IngredientController
{
    private static List<IngredientData> datas = new List<IngredientData>();

    public static void LoadData(List<IngredientLoaderObject> objects)
    {
        foreach (var item in objects)
        {
            datas.Add(new IngredientData(item));
        }
    }

    public static Ingredient Get(string name)
    {
        return new Ingredient(datas.Find(a => a.Name == name) ?? throw new Exception("Missing ingredient! " + name));
    }

    public static IngredientData GetData(string name)
    {
        return datas.Find(a => a.Name == name) ?? throw new Exception("Missing ingredient! " + name);
    }
}
