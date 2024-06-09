using Godot;
using System;

public partial class HitDetection : Area2D
{
	private AudioStreamPlayer soundPlayer;
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	private void OnBodyEntered(Node body)
	{	
		if (body is Enemy enemy)
		{
			soundPlayer = GetNode<AudioStreamPlayer>("Sounds/Hit");
			soundPlayer.Play();
			enemy.Die();
		}
	}
}
