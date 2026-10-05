using Godot;

namespace RootGame;

public static class Resource
{
    public static readonly PackedScene SporeScene = GD.Load<PackedScene>("res://src/spore.tscn");
    public static readonly PackedScene WaterScene = GD.Load<PackedScene>("res://src/water.tscn");
    public static readonly PackedScene EnergyScene = GD.Load<PackedScene>("res://src/energy.tscn");
}