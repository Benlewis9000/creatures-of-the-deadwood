class_name Water extends RigidBody2D

var _target_mushroom: Mushroom

func on_drag_end() -> void:
	if _target_mushroom != null:
		_target_mushroom.water_level += 1
		hide()
		queue_free()

func on_body_entered(body: Node) -> void:
	if body is Mushroom:
		_target_mushroom = body

func on_body_exited(body: Node) -> void:
	if body == _target_mushroom:
		_target_mushroom = null
