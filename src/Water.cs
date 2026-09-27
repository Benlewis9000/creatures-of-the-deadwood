#nullable enable
using Godot;

namespace RootGame;

public partial class Water : RigidBody2D
{
	private Mushroom? _targetMushroom;

	private void OnDragEnd()
	{
		if (_targetMushroom is not null)
		{
			_targetMushroom.WaterLevel++;
			Hide();
			QueueFree();
		}
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body is Mushroom mushroom)
		{
			_targetMushroom = mushroom;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == _targetMushroom)
		{
			_targetMushroom = null;
		}
	}
}