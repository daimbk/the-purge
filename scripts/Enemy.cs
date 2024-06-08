using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public int Speed = 100;
	[Export] public float AttackRange = 15.0f;
	
	private Node2D player;
	private AnimatedSprite2D animation;
	private bool isAttacking = false;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Main/Player");
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Calculate the direction to the player
		Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		if (distanceToPlayer <= AttackRange)
		{
			// Stop moving and attack the player
			Velocity = Vector2.Zero;
			if (!isAttacking)
			{
				AttackPlayer();
			}
		}
		else
		{
			// Move towards the player
			Vector2 velocity = direction * Speed;
			Velocity = velocity;
			animation.Play("walk");
			isAttacking = false;
		}
		
		MoveAndSlide();
	}
	
	private void AttackPlayer()
	{
		isAttacking = true;
		animation.Play("attack");
		((Player)player).GetHit();
	}
}
