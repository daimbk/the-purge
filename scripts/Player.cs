using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public const int Speed = 300;
	private AnimatedSprite2D animation;
	public Label healthUI;
	
	private bool isHit = false;
	private bool dead = false;
	private int health = 3;
	

	public override void _Ready()
	{
		// get the AnimatedSprite2D node
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));
		
		healthUI = GetNode<Label>("/root/Main/Player/HealthUI");
	}

	public override void _PhysicsProcess(double delta)
	{
		healthUI.Text = "Health: " + health;
		
		if (!dead && !isHit)
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
	
	public void GetHit()
	{
		if (!isHit)  // ensure that the player is not already in the hit animation
		{
			isHit = true;
			animation.Play("hit");
			health -= 1;
			if (health == 0)
			{
				Die();
			}
		}
	}
	
	private void Die()
	{
		dead = true;
		animation.Play("death");
	}
	
	private void OnAnimationFinished()
	{
		if (animation.Animation == "hit")
		{
			isHit = false;
			if (health == 0)
			{
				Die();
			}
		}
		else if (animation.Animation == "death")
		{
			GetTree().ChangeSceneToFile("res://scenes/GameOver.tscn");
		}
	}
}
