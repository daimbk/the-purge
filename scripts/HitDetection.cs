using Godot;
using System;

public partial class HitDetection : Area2D
{
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	private void OnBodyEntered(Node body)
	{	
		if (body is Enemy enemy)
		{
			enemy.Die();
		}
	}
}
