using System;
using Godot;

namespace RootGame;

public partial class Mushroom : RigidBody2D
{
    [Signal] public delegate void MonsterFedEventHandler();
    
    private const int RequiredEnergyLevel = 2;
    private const int RequiredWaterLevel = 2;

    private PointLight2D _light;
    private GodotObject _draggable;
    private Monster _targetMonster;
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
        _draggable = GetNode<GodotObject>("Draggable") ?? throw new NullReferenceException("No Draggable for Mushroom"); 
    }

    private void TrySaturate()
    {
        if (!IsSaturated)
        {
            return;
        }
        
        _light.Enabled = true;
        _draggable.Set("disabled", false);
    }

    private void OnDragStart()
    {
        Freeze = false;
    }

    private void OnDragEnd()
    {
        if (_targetMonster is null)
        {
            return;
        }

        EmitSignalMonsterFed();
        _targetMonster.Deactivate();
        Hide();
        QueueFree();
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (area is Monster monster)
        {
            _targetMonster = monster;
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area == _targetMonster)
        {
            _targetMonster = null;
        }
    }
}