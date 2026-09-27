using System;
using Godot;

namespace RootGame;

public partial class Mushroom : RigidBody2D
{
    private const int RequiredEnergyLevel = 2;
    private const int RequiredWaterLevel = 2;
    
    private PointLight2D _light;
    private Draggable _draggable;
    private int _energyLevel;
    private int _waterLevel;

    public int EnergyLevel
    {
        get => _energyLevel;
        set
        {
            _energyLevel = value;
            TrySaturate();
        }
    }
    
    public int WaterLevel
    {
        get => _waterLevel;
        set
        {
            _waterLevel = value;
            TrySaturate();
        }
    }

    private bool IsSaturated => _energyLevel >= RequiredEnergyLevel && _waterLevel >= RequiredWaterLevel;
    
    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("PointLight2D") ?? throw new NullReferenceException("No PointLight2D for Mushroom"); 
        _draggable = GetNode<Draggable>("Draggable") ?? throw new NullReferenceException("No Draggable for Mushroom"); 
    }

    private void TrySaturate()
    {
        if (!IsSaturated)
        {
            return;
        }
        
        _light.Enabled = true;
        _draggable.Disabled = false;
    }
    
    private void OnDragStart()
    {
        Freeze = false;
    }
}