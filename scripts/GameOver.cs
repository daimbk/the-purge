using Godot;
using System;

public partial class GameOver : Control
{
	public override void _Ready()
	{
		GetNode<Button>("/root/GameOver/Menu/VBoxContainer/PlayAgain").GrabFocus();
	}

	private void _on_play_again_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
	}
	
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
