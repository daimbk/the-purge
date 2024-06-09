using Godot;
using System;

public partial class Main : Node2D
{
	private AudioStreamPlayer soundPlayer;

	public override void _Ready()
	{
		soundPlayer = GetNode<AudioStreamPlayer>("Sounds/MainTheme");
		soundPlayer.Play();
	}

	public override void _Process(double delta)
	{
	}
}
