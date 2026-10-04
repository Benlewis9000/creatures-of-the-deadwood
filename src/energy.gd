class_name Energy extends RigidBody2D

const _mushroom = preload("res://src/Mushroom.cs")

var _target_mushroom: _mushroom

func on_drag_end() -> void:
	if _target_mushroom != null:
		_target_mushroom.EnergyLevel += 1
		hide()
		queue_free()

func on_body_entered(body: Node) -> void:
	if body is _mushroom:
		_target_mushroom = body

func on_body_exited(body: Node) -> void:
	if body == _target_mushroom:
		_target_mushroom = null
