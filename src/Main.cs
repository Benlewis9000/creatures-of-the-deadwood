using Godot;

namespace RootGame;

public partial class Main : Node2D
{
	private PackedScene _levelScene = GD.Load<PackedScene>("res://src/level.tscn");
	
	public override void _Ready()
	{
		GetNode<Level>("Level").RestartGame += OnGameRestart;
	}

	private void OnGameRestart()
	{
		var currentLevel = GetNode<Level>("Level");
		RemoveChild(currentLevel);
		currentLevel.QueueFree();
		
		var nextLevel = _levelScene.Instantiate<Level>();
		nextLevel.RestartGame += OnGameRestart;
		AddChild(nextLevel);
	}
}