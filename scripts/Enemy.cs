using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public int Speed = 100;
	[Export] public float AttackRange = 15.0f;
	
	[Export] public PackedScene HealthOrbScene;
	[Export] public PackedScene PowerOrbScene;
	[Export] public PackedScene EnergyOrbScene;
	
	private Node2D player;
	private AnimatedSprite2D animation;
	private AudioStreamPlayer soundPlayer;
	private bool isAttacking = false;
	private bool isDying = false;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Main/Player");
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isDying)
		{
			// don't perform any actions if the enemy is dying
			return;
		}
		
		// calculate the direction to the player
		Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		// adjust sprite orientation
		if (direction.X > 0)
		{
			animation.FlipH = true;
		}
		else if (direction.X < 0)
		{
			animation.FlipH = false;
		}
		
		if (distanceToPlayer <= AttackRange)
		{
			// stop moving and attack the player
			Velocity = Vector2.Zero;
			if (!isAttacking)
			{
				AttackPlayer();
			}
		}
		else
		{
			// move towards the player
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
		((Player)player).GetHit();
		soundPlayer = GetNode<AudioStreamPlayer>("Sounds/Attack");
		soundPlayer.Play();
		animation.Play("attack");
	}
	
	public void Die()
	{
		isDying = true;
		soundPlayer = GetNode<AudioStreamPlayer>("Sounds/Damage");
		soundPlayer.Play();
		animation.Play("death");
		CallDeferred(nameof(DropOrbs));
	}
	
	private void DropOrbs()
	{
		// health orb drop with 20% drop chance
		if (GD.Randf() < 0.2f)
		{
			var healthOrbInstance = (Node2D)HealthOrbScene.Instantiate();
			healthOrbInstance.GlobalPosition = GlobalPosition;
			GetParent().AddChild(healthOrbInstance);
		}
		
		// drop 8 PowerOrbs
		for (int i = 0; i < 8; i++)
		{
			var powerOrbInstance = (Node2D)PowerOrbScene.Instantiate();
			powerOrbInstance.GlobalPosition = GlobalPosition + new Vector2(GD.Randf() * 20 - 10, GD.Randf() * 20 - 10);
			GetParent().AddChild(powerOrbInstance);
		}
		
		 // drop 2 EnergyOrbs
		for (int i = 0; i < 2; i++)
		{
			var energyOrbInstance = (Node2D)EnergyOrbScene.Instantiate();
			energyOrbInstance.GlobalPosition = GlobalPosition + new Vector2(GD.Randf() * 20 - 10, GD.Randf() * 20 - 10);
			GetParent().AddChild(energyOrbInstance);
		}
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
