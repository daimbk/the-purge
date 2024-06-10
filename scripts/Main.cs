using Godot;
using System;

public partial class Main : Node2D
{
	private Timer survivalTimer;
	private AudioStreamPlayer soundPlayer;
	private Label timerUI;
	private float elapsedTime = 0.0f;

	public override void _Ready()
	{
		survivalTimer = new Timer();
		AddChild(survivalTimer);
		survivalTimer.WaitTime = 1.0f;
		survivalTimer.OneShot = false;
		survivalTimer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));
		survivalTimer.Start();

		timerUI = GetNode<Label>("/root/Main/Player/TimeDisplay");

		soundPlayer = GetNode<AudioStreamPlayer>("Sounds/MainTheme");
		soundPlayer.Play();
	}

	public override void _Process(double delta)
	{
		timerUI.Text = FormatTime(elapsedTime);
	}

	private void OnTimerTimeout()
	{
		elapsedTime += 1.0f;
	}

	private string FormatTime(float time)
	{
		int totalSeconds = Mathf.FloorToInt(time);
		int hours = totalSeconds / 3600;
		int minutes = (totalSeconds % 3600) / 60;
		int seconds = totalSeconds % 60;

		if (hours > 0)
		{
			return $"{hours}h {minutes}m {seconds}s";
		}
		else if (minutes > 0)
		{
			return $"{minutes}m {seconds}s";
		}
		else
		{
			return $"{seconds}s";
		}
	}

	public float GetElapsedTime()
	{
		return elapsedTime;
	}
}
