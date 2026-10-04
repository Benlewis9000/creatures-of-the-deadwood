class_name Draggable extends Area2D

@export var shape: Shape2D
@export var drag_speed: int = 30
@export var disabled: bool = false

signal drag_start
signal drag_end

var _parent: RigidBody2D
var _is_dragging: bool

func _ready() -> void:
	assert(get_parent().is_class("RigidBody2D"), "Draggable parent must be RigidBody2D but was " + get_parent().get_class())
	assert(shape != null, "Draggable's Shape must not be empty")
	_parent = get_parent()
	$CollisionShape2D.shape = shape

func _process(delta: float) -> void:
	if disabled:
		return
	process_drag(delta)

func on_input_event(_viewport: Node, event: InputEvent, _shape_index: int) -> void:
	if disabled:
		return
	if event.is_action_pressed("mouse_left"):
		start_drag()

func _unhandled_input(event: InputEvent) -> void:
	if disabled:
		return
	if event.is_action_released("mouse_left"):
		end_drag()

func start_drag() -> void:
	if(_is_dragging):
		return
	_is_dragging = true
	_parent.linear_velocity = Vector2.ZERO
	drag_start.emit()

func end_drag() -> void:
	if(!_is_dragging):
		return
	_is_dragging = false
	var distance = (get_global_mouse_position() - _parent.position).floor()
	_parent.linear_velocity += distance * 4
	drag_end.emit()

func process_drag(delta: float) -> void:
	if(!_is_dragging):
		return
	_parent.position = _parent.position.lerp(get_global_mouse_position(), drag_speed * delta)

func on_mouse_entered() -> void:
	if disabled:
		return
	Input.set_default_cursor_shape(Input.CURSOR_POINTING_HAND)

func on_mouse_exited() -> void:
	if disabled:
		return
	Input.set_default_cursor_shape()
