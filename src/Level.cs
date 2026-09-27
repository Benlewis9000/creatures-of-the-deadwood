using System;
using Godot;

namespace RootGame;

public partial class Level : Node2D
{
	private Timer _resourceTimer;
	
	public override void _Ready()
	{
		_resourceTimer = GetNode<Timer>("ResourceTimer") ?? throw new NullReferenceException("No ResourceTimer for level");
	}

	public override void _Process(double delta)
	{
	}

	private void OnResourceTimerTimeout()
	{
		var resourceRandom = GD.Randi() % 6;
		var resource = resourceRandom switch
		{
			< 1 => CreateSporeScene(),
			< 3 => CreateEnergyScene(),
			_ => CreateWaterScene()
		};
		
		AddChild(resource);
		RestartResourceTimer();
	}
	
	private void RestartResourceTimer()
	{
		// Spawnrate of 3 to 10 seconds
		var interval = 3 + GD.Randf() % 10 ;
		_resourceTimer.Start(interval);
	}

	private Node2D CreateSporeScene()
	{
		var spore = CreateResourceScene<Spore>(Resource.SporeScene);
		spore.SporePlanted += (position) =>
		{
			var mushroom = GD.Load<PackedScene>("res://src/mushroom.tscn").Instantiate<Node2D>();
			mushroom.Position = position;
			AddChild(mushroom);
		};
		
		return spore;
	}

	private Node2D CreateEnergyScene()
	{
		return CreateResourceScene<Node2D>(Resource.EnergyScene);
	}

	private Node2D CreateWaterScene()
	{
		return CreateResourceScene<Node2D>(Resource.WaterScene);
	}

	private T CreateResourceScene<T>(PackedScene resourceScene) where T : Node2D
	{
		var scene = resourceScene.Instantiate<T>();
		var spawnLocation = GetNode<PathFollow2D>("ResourceSpawner/Path");
		spawnLocation.ProgressRatio = GD.Randf();
		scene.Position = spawnLocation.Position;
		return  scene;
	}
}