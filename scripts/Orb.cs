using Godot;
using System;

public partial class Orb : CharacterBody2D
{
	private Vector2 targetPosition;
	private bool isMoving = false;
	private float speed = 200.0f;

	public override void _Process(double delta)
	{
		if (isMoving)
		{
			Vector2 direction = (targetPosition - GlobalPosition).Normalized();
			Vector2 velocity = direction * speed * (float)delta;
			GlobalPosition += velocity;

			// stop moving if close enough to the target
			if (GlobalPosition.DistanceTo(targetPosition) < 5.0f)
			{
				isMoving = false;
				QueueFree();
			}
		}
	}

	public void StartMovingTowards(Node2D target)
	{
		targetPosition = target.GlobalPosition;
		isMoving = true;
	}
}
