class_name Main extends Node2D

const _LEVEL_SCENE = preload("res://src/level.tscn")

func _ready() -> void:
	$Level.restart_game.connect(_on_game_restart)

func _on_game_restart() -> void:
	var current_level = $Level
	remove_child(current_level)
	current_level.queue_free()
	
	var next_level: Level = _LEVEL_SCENE.instantiate()
	next_level.restart_game.connect(_on_game_restart)
	add_child(next_level)
