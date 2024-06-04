using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int Speed = 100;
	private Node2D player;
	private AnimatedSprite2D animation;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Main/Player");
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
		Vector2 velocity = direction * Speed;
		Velocity = velocity;
		MoveAndSlide();
		animation.Play("walk");
	}
}
