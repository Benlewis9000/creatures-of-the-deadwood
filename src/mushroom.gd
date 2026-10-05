class_name Mushroom extends RigidBody2D

const _REQUIRED_ENERGY_LEVEL = 2
const _REQUIRED_WATER_LEVEL = 2

signal monster_fed

@onready var _light: PointLight2D = $PointLight2D
@onready var _draggable: Draggable = $Draggable

var _target_monster: Monster = null

var _energy_level: int = 0
var energy_level:
	get:
		return _energy_level
	set(value):
		_energy_level = value
		_try_saturate()

var _water_level: int = 0
var water_level:
	get:
		return _water_level
	set(value):
		_water_level = value
		_try_saturate()

func _is_saturated() -> bool:
	return energy_level >= _REQUIRED_ENERGY_LEVEL && water_level >= _REQUIRED_WATER_LEVEL

func _try_saturate() -> void:
	if !_is_saturated():
		return
	_light.enabled = true
	_draggable.disabled = false

func _on_drag_start() -> void:
	freeze = false

func _on_drag_end() -> void:
	if _target_monster == null:
		return
	monster_fed.emit()
	_target_monster.deactivate()
	hide()
	queue_free()

func _on_area_entered(area: Area2D) -> void:
	if area is Monster:
		_target_monster = area

func _on_area_exited(area: Area2D) -> void:
	if area == _target_monster:
		_target_monster = null
