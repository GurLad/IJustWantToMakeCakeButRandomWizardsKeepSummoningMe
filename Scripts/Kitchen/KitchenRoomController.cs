using Godot;
using System;
using System.Linq;

public partial class KitchenRoomController : Node
{
    [Export] private Node2D[] rooms;
    [Export] private int firstRoom;

    private int currentRoom;

    public override void _Ready()
    {
        base._Ready();
        rooms.ToList().ForEach(a => a.Visible = false);
        rooms[currentRoom = firstRoom].Visible = true;
    }

    public void MoveRoom(int direction)
    {
        rooms[currentRoom].Visible = false;
        rooms[currentRoom = (currentRoom + rooms.Length + direction) % rooms.Length].Visible = true;
    }
}
