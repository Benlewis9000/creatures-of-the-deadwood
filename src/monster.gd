class_name Monster extends Area2D

@onready var _active_timer: Timer = $ActiveTimer
@onready var _sprite: AnimatedSprite2D = $AnimatedSprite2D

var _is_activated: bool = false

func activate() -> void:
	if _is_activated:
		return
	_is_activated = true
	visible = true
	_active_timer.start()
	var animation_name: String
	match randi() % 2:
		0:
			animation_name = "eyes_0"
		1:
			animation_name = "eyes_1"
	assert(animation_name != null)
	_sprite.play(animation_name)

func deactivate() -> void:
	_is_activated = false
	visible = false

func _on_active_time_timeout() -> void:
	deactivate()
