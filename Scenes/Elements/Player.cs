using Godot;
using System;

public class Player : KinematicBody2D
{
	private int _speed = 200;
	private int _health = 100;
	private Vector2 _velocity = new Vector2();
	
	public override void _Ready()
	{
		// cock
	}

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		_velocity = MoveAndSlide(_velocity);
		base._PhysicsProcess(delta);
	}

	private void GetInput()
	{
		_velocity = Vector2.Zero;
		if (Input.IsActionPressed("player_up"))
		{
			_velocity.y = -1;
		}
		if (Input.IsActionPressed("player_down"))
		{
			_velocity.y = 1;
		}
		if (Input.IsActionPressed("player_left"))
		{
			_velocity.x = -1;
		}
		if (Input.IsActionPressed("player_right"))
		{
			_velocity.x = 1;
		}

		_velocity = _velocity.Normalized() * _speed;
	}

}
