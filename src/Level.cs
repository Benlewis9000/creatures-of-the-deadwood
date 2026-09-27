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
		var resourceScene = resourceRandom switch
		{
			< 1 => Resource.SporeScene,
			< 3 => Resource.EnergyScene,
			_ => Resource.WaterScene
		};
		var scene = resourceScene.Instantiate<Node2D>();
		
		var spawnLocation = GetNode<PathFollow2D>("ResourceSpawner/Path");
		spawnLocation.ProgressRatio = GD.Randf();
		scene.Position = spawnLocation.Position;
		
		AddChild(scene);
		RestartResourceTimer();
	}
	
	private void RestartResourceTimer()
	{
		// Spawnrate of 3 to 10 seconds
		var interval = 3 + GD.Randf() % 10 ;
		_resourceTimer.Start(interval);
	}
}