using Godot;
using System;
using System.Linq;

public partial class KitchenRoomController : Node
{
    [Export] private Node2D[] rooms;
    [Export] private Node2D recipeRoom;
    [Export] private Node2D global;
    [Export] private int firstRoom;
    [Export] private FadeTransition fadeTransition;

    public int CurrentRoom { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        rooms.ToList().ForEach(a => a.Visible = false);
        rooms[CurrentRoom = firstRoom].Visible = true;
    }

    public void ToggleRecipeRoom(bool on, bool animate)
    {
        if (animate)
        {
            fadeTransition.Transition(() =>
            {
                rooms[CurrentRoom].Visible = global.Visible = !on;
                recipeRoom.Visible = on;
            }, null);
        }
        else
        {
            rooms[CurrentRoom].Visible = global.Visible = !on;
            recipeRoom.Visible = on;
        }
    }

    public void SetRoom(int room)
    {
        rooms[CurrentRoom].Visible = false;
        rooms[CurrentRoom = room].Visible = true;
    }

    public void MoveRoom(int direction)
    {
        fadeTransition.Transition(() =>
        {
            rooms[CurrentRoom].Visible = false;
            rooms[CurrentRoom = (CurrentRoom + rooms.Length + direction) % rooms.Length].Visible = true;
        }, null);
    }
}
