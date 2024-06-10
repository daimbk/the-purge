using Godot;
using System;

public partial class Vaccuum : Area2D
{
	private AudioStreamPlayer soundPlayer;
	private Player player;

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		player = GetNode<Node2D>("/root/Main/Player") as Player;
		soundPlayer = GetNode<AudioStreamPlayer>("Sounds/Pickup");
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Orb orb)
		{
			if (body is HealthOrb)
			{
				player.IncreaseHealth();
			}
			else if (body is PowerOrb)
			{
				player.IncreasePower();
			}
			else if (body is EnergyOrb)
			{
				player.IncreaseEnergy();
			}
			
			orb.StartMovingTowards(player);
			soundPlayer.Play();
		}
	}
}
