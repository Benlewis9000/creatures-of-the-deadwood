using System;
using Godot;

namespace RootGame;

public partial class Level : Node2D
{
	private Timer _resourceTimer;
	private Timer _monsterTimer;
	private Monster[] _monsters =  new Monster[6];
	
	public override void _Ready()
	{
		_resourceTimer = GetNode<Timer>("ResourceTimer") ?? throw new NullReferenceException("No ResourceTimer for level");
		_monsterTimer = GetNode<Timer>("MonsterTimer") ?? throw new NullReferenceException("No MonsterTimer for level");
		for (int i = 0; i < 6; i++)
		{
			_monsters[i] = GetNode<Monster>("Monster" + i);
		}
	}

	public override void _Process(double delta)
	{
	}

	private void OnResourceTimerTimeout()
	{
		var resourceRandom = GD.Randi() % 5;
		var resource = resourceRandom switch
		{
			< 1 => CreateSporeScene(),
			< 3 => CreateEnergyScene(),
			_ => CreateWaterScene()
		};
		
		var interval = 3 + GD.Randf() % 10 ;
		_resourceTimer.Start(interval);
		
		AddChild(resource);
	}

	private void OnMonsterTimerTimeout()
	{
		var monsterRandom = GD.Randi() % 5;
		_monsters[monsterRandom].Activate();
		
		var interval = 10 + GD.Randi() % 25 ;
		_monsterTimer.Start(interval);
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