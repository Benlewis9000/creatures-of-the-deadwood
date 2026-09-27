using System;
using Godot;

namespace RootGame;

public partial class Draggable : Area2D
{
	[Export] public required Shape2D Shape { get; set; }

	[Export] public int DragSpeed { get; set; } = 100;

	[Export] public bool Disabled { get; set; }
	
	[Signal] public delegate void DragStartEventHandler();
	
	[Signal] public delegate void DragEndEventHandler();

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
			StartDrag();
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
		
		// End drag is an unhandled input since the mouse may not be within the collision shape on release
		if (eventMouseButton.IsActionReleased("mouse_left"))
		{
			EndDrag();
		}
	}

	private void StartDrag()
	{
		if (_isDragging)
		{
			return;
		}
		
		_isDragging = true;
		
		var node = GetParent<RigidBody2D>();
		node.LinearVelocity = Vector2.Zero;
		
		EmitSignalDragStart();
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
		
		EmitSignalDragEnd();
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