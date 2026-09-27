using System;
using Godot;

namespace RootGame;

public partial class Monster : Area2D
{
	private Timer _activeTimer;
	private bool _isActivated;

	public override void _Ready()
	{
		_activeTimer = GetNode<Timer>("ActiveTimer") ?? throw new NullReferenceException("No ActiveTimer for monster");
	}

	public void Activate()
	{
		if (_isActivated)
		{
			return;
		}
		
		_isActivated = true;
		Visible = true;
		_activeTimer.Start();

		var animationName = (GD.Randi() % 2) switch
		{
			< 1 => "eyes_0",
			_ => "eyes_1"
		};
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play(animationName);
	}
	
	private void OnActiveTimerTimeout()
	{
		_isActivated = false;
		Visible = false;
	}
}