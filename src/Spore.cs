using Godot;

namespace RootGame;

public partial class Spore : RigidBody2D, ITreeEnterable
{
    [Signal] public delegate void SporePlantedEventHandler(Vector2 position);
    
    private bool _isInTreeArea;
    
    public void OnTreeEnter()
    {
        _isInTreeArea = true;
    }

    public void OnTreeExit()
    {
        _isInTreeArea = false;
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