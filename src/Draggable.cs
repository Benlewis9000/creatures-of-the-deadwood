using System;
using Godot;

namespace RootGame;

public partial class Draggable : Area2D
{
	[Export] public required Shape2D Shape { get; set; }

	[Export] public int DragSpeed { get; set; } = 100;

	[Export] public bool Disabled { get; set; }

	private RigidBody2D _parent;
	private bool _isDragging;

	public override void _Ready()
	{
		_parent = GetParent<RigidBody2D>() ?? throw new NullReferenceException("Parent is not a RigidBody2D");
		GetNode<CollisionShape2D>("CollisionShape2D").SetShape(Shape);
	}

	public override void _Process(double delta)
	{
		if (Disabled)
		{
			return;
		}

		ProcessDrag(delta);
	}

	private void OnInputEvent(Node viewport, InputEvent @event, int shapeIndex)
	{
		if (Disabled)
		{
			return;
		}

		if (@event is not InputEventMouseButton eventMouseButton)
		{
			return;
		}

		if (eventMouseButton.IsActionPressed("mouse_left"))
		{
			BeginDrag();
		}

		if (eventMouseButton.IsActionReleased("mouse_left"))
		{
			EndDrag();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Disabled)
		{
			return;
		}
		
		if (@event is not InputEventMouseButton eventMouseButton)
		{
			return;
		}
		
		if (eventMouseButton.IsActionReleased("mouse_left"))
		{
			EndDrag();
		}
	}

	private void BeginDrag()
	{
		if (_isDragging)
		{
			return;
		}
		
		_isDragging = true;
		
		var node = GetParent<RigidBody2D>();
		node.LinearVelocity = Vector2.Zero;
	}

	private void EndDrag()
	{
		if (!_isDragging)
		{
			return;
		}
		
		_isDragging = false;
		
		var mousePos = GetGlobalMousePosition();
		var nodePos = _parent.Position;
		var distance = (mousePos - nodePos).Floor();
		var vector = distance.Normalized();
		_parent.LinearVelocity += vector * (distance.Length() * 4);
	}
	
	private void ProcessDrag(double delta)
	{
		if (!_isDragging)
		{
			return;
		}
		
		var mousePos = GetGlobalMousePosition();
		var nodePos = _parent.Position;
		var nextPos = nodePos.Lerp(mousePos, DragSpeed * (float) delta);
		_parent.Position = nextPos;
	}
}