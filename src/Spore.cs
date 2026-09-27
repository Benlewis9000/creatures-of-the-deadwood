using Godot;

namespace RootGame;

public partial class Spore : RigidBody2D
{
    [Signal] public delegate void SporePlantedEventHandler(Vector2 position);
    
    private bool _isInTreeArea;

    private void OnAreaEntered(Area2D area)
    {
        if (area is Tree)
        {
            _isInTreeArea = true;
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is Tree)
        {
            _isInTreeArea = false;
        }
    }

    private void OnDragEnd()
    {
        if (!_isInTreeArea)
        {
            return;
        }
        
        EmitSignalSporePlanted(Position);
        Hide();
        QueueFree();
    }
}