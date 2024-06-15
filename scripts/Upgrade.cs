using Godot;
using System;

public partial class Upgrade : VBoxContainer
{
	private AudioStreamPlayer upgradeSoundPlayer;
	private AudioStreamPlayer buttonPressPlayer;
	private AudioStreamPlayer buttonSwitchPlayer;
	
	private Player player;
	private Shuriken shuriken;
	private PackedScene RedShurikenScene;

	public override void _Ready()
	{
		player = GetNode<Player>("/root/Main/Player");
		shuriken = GetNode<Shuriken>("/root/Main/Player/Shuriken");
		RedShurikenScene = (PackedScene)ResourceLoader.Load("res://scenes/RedShuriken.tscn");
		
		// audio players
		upgradeSoundPlayer = GetNode<AudioStreamPlayer>("Sounds/UpgradeSound");
		buttonPressPlayer = GetNode<AudioStreamPlayer>("Sounds/ButtonPress");
		buttonSwitchPlayer = GetNode<AudioStreamPlayer>("Sounds/ButtonSwitch");
		
		// press signals
		GetNode<Button>("Button1").Connect("pressed", new Callable(this, nameof(OnButton1Pressed)));
		GetNode<Button>("Button2").Connect("pressed", new Callable(this, nameof(OnButton2Pressed)));
		GetNode<Button>("Button3").Connect("pressed", new Callable(this, nameof(OnButton3Pressed)));
		
		// focus signals
		GetNode<Button>("Button1").Connect("focus_entered", new Callable(this, nameof(OnButtonFocusEntered)));
		GetNode<Button>("Button2").Connect("focus_entered", new Callable(this, nameof(OnButtonFocusEntered)));
		GetNode<Button>("Button3").Connect("focus_entered", new Callable(this, nameof(OnButtonFocusEntered)));
		GetNode<Button>("Button1").GrabFocus();
		
		shuriken = GetNode<Shuriken>("/root/Main/Player/Shuriken");
		upgradeSoundPlayer.Play();
	}

	public void ShowUpgradeScreen(Player player)
	{
		GetTree().Paused = true;
		Visible = true;
	}

	private void HideUpgradeScreen()
	{
		GetTree().Paused = false;
		Visible = false;
	}

	private void OnButton1Pressed()
	{
		buttonPressPlayer.Play();
		shuriken.increaseSpeed();
		HideUpgradeScreen();
	}
	
	private void OnButton2Pressed()
	{
		buttonPressPlayer.Play();
		
		// spawn second shuriken
		var redShuriken = (Node2D)RedShurikenScene.Instantiate();
		player.AddChild(redShuriken);
		//redShuriken.Position = new Vector2(0, 0);
		
		HideUpgradeScreen();
	}
	
	private void OnButton3Pressed()
	{
		buttonPressPlayer.Play();
		player.extraLife = true;
		HideUpgradeScreen();
	}
	
	private void OnButtonFocusEntered()
	{
		buttonSwitchPlayer.Play();
	}
}
