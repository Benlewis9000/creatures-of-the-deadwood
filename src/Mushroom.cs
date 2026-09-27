using Godot;

namespace RootGame;

public partial class Mushroom : RigidBody2D
{
    private void OnDragStart()
    {
        Freeze = false;
    }
}