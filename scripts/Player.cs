using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	private AnimatedSprite2D animation;

	public override void _Ready()
	{
		// get the AnimatedSprite2D node
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// get the horizontal and vertical input direction
		float directionX = Input.GetAxis("move_left", "move_right");
		float directionY = Input.GetAxis("move_up", "move_down");

		// set the velocity based on input
		velocity.X = directionX * Speed;
		velocity.Y = directionY * Speed;

		// handle sprite flipping based on horizontal direction
		if (directionX < 0)
		{
			animation.FlipH = true;
		}
		else if (directionX > 0)
		{
			animation.FlipH = false;
		}

		// play appropriate animations based on movement
		if (velocity == Vector2.Zero)
		{
			animation.Play("idle");
		}
		else
		{
			animation.Play("walk");
		}

		// update the velocity and move the player
		Velocity = velocity;
		MoveAndSlide();
	}
}
