using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("/root/MainMenu/Menu/VBoxContainer/Play").GrabFocus();
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
