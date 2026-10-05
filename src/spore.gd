class_name Spore extends RigidBody2D

signal spore_planted(position: Vector2)

var _is_in_tree: bool

func _on_area_entered(area: Area2D) -> void:
	if area is TreeArea:
		_is_in_tree = true

func _on_area_exited(area: Area2D) -> void:
	if area is TreeArea:
		_is_in_tree = false

func _on_drag_end() -> void:
	if !_is_in_tree:
		return
	spore_planted.emit(position)
	hide()
	queue_free()
