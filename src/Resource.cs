using Godot;

namespace RootGame;

public static class Resource
{
    public static readonly GDScript SporeScene = GD.Load<GDScript>("res://src/spore.gd");
    public static readonly GDScript WaterScene = GD.Load<GDScript>("res://src/water.gd");
    public static readonly GDScript EnergyScene = GD.Load<GDScript>("res://src/energy.gd");
}