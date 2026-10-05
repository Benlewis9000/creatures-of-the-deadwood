class_name Level extends Node2D

signal restart_game

const _MAX_SATURATION = 90

@onready var _progress_bar: TextureProgressBar = $TextureProgressBar
@onready var _score_label: Label = $ScoreLabel
@onready var _resource_timer: Timer = $ResourceTimer
@onready var _monster_timer: Timer = $MonsterTimer
@onready var _score_timer: Timer = $ScoreTimer

var _is_game_over: int
var _monsters: Array[Monster] = []

var _score: int = 0
var score: int:
	get:
		return _score
	set(value):
		_score = value
		_score_label.text = _format_score(_score)

var _saturation: int = _MAX_SATURATION
var saturation: int:
	get:
		return _saturation
	set(value):
		_saturation = _MAX_SATURATION if value > _MAX_SATURATION else value
		_progress_bar.value = _saturation
		if _saturation <= 0:
			_do_game_over()
		

func _ready() -> void:
	_progress_bar.max_value = _MAX_SATURATION
	_score_label.text = _format_score(score)
	for i in range(0, 6):
		_monsters.push_back(get_node("Monster%d" % i))

func _unhandled_input(event: InputEvent) -> void:
	if !_is_game_over:
		return
	if event.is_action_pressed("restart"):
		restart_game.emit()

func _do_game_over() -> void:
	_is_game_over = true
	_resource_timer.stop()
	_monster_timer.stop()
	_score_timer.stop()
	$MusicSound.stop()
	$GameOverSound.play()
	$GameOverCard/ScoreLabel.text = _format_score(score)
	$GameOverCard.show()

func _on_resource_timer_timeout() -> void:
	var resource_id = randi() % 5
	var resource = null
	match resource_id:
		0:
			resource = _create_spore_scene()
		1, 2:
			resource = _create_energy_scene()
		3, 4:
			resource = _create_water_scene()
	assert(resource != null)
	# TODO compare this to C# spawnrate (which was likely erroneous)
	var interval: float = 1.0 + randf()
	_resource_timer.start(interval)
	add_child(resource)
	
func _on_monster_timer_timeout() -> void:
	var monster_id = randi() % 5
	_monsters[monster_id].activate()
	var interval = 5 + randi() % 15
	_monster_timer.start(interval)

func _on_score_timer_timeout() -> void:
	score += 1
	saturation -= 1

func _create_spore_scene() -> Spore:
	var spore: Spore = _create_resource_scene(preload("res://src/spore.tscn"))
	spore.spore_planted.connect(_on_spore_planted)
	return spore

func _on_spore_planted(planted_position: Vector2) -> void:
	var mushroom: Mushroom = preload("res://src/mushroom.tscn").instantiate()
	mushroom.position = planted_position
	add_child(mushroom)
	mushroom.monster_fed.connect(_on_monster_fed)

func _on_monster_fed() -> void:
	saturation += 10

func _create_energy_scene() -> Energy:
	return _create_resource_scene(preload("res://src/energy.tscn")) as Energy

func _create_water_scene() -> Water:
	return _create_resource_scene(preload("res://src/water.tscn")) as Water

func _create_resource_scene(packed_scene: PackedScene) -> Node2D:
	var scene = packed_scene.instantiate() as Node2D
	var spawn_path = $ResourceSpawner/Path
	spawn_path.progress_ratio = randf()
	scene.position = spawn_path.position
	return scene

func _format_score(s: int) -> String:
	return "%04d" % s
