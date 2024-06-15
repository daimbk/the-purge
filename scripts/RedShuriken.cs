using Godot;
using System;

public partial class RedShuriken : Node2D
{
	[Export] public float RotationSpeed = 20.0f;
	[Export] public float orbitRadius = 150f;
	[Export] public float orbitSpeed = 10.0f;
	private float angle = 0f;
	
	private Node2D player;
	private Sprite2D shuriken;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Main/Player");
		shuriken = GetNode<Sprite2D>("Sprite2D");
	}
	
	public override void _Process(double delta)
	{
		// spin shuriken
		shuriken.Rotation += RotationSpeed * (float)delta;
		
		// update the angle based on the orbit speed
		angle += orbitSpeed * (float)delta;

		// calculate the weapon's position relative to the player
		Vector2 offset = new Vector2(
			Mathf.Cos(angle) * orbitRadius * player.Scale.X,
			Mathf.Sin(angle) * orbitRadius * player.Scale.Y
		);

		// set the weapon's position relative to the player
		Position = offset;
	}
	
	public void increaseSpeed()
	{
		orbitSpeed += 5;
	}
}
