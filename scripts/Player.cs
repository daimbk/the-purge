using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public const int Speed = 300;
	private AnimatedSprite2D animation;
	private AudioStreamPlayer soundPlayer;
	public Label healthUI;
	public Label energyUI;
	public Label powerUI;
	public Label levelUI;
	private Main mainScene;
	private PackedScene upgradeScene;

	private bool isHit = false;
	private bool dead = false;
	public bool extraLife = false;
	
	// 10 energy = 1 AOE attack
	// 100 power orbs = 1 level up
	private int health = 3;
	private int energy = 0;
	private int level = 0;
	private int power = 99;

	public override void _Ready()
	{
		// get the AnimatedSprite2D node
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));
		
		healthUI = GetNode<Label>("/root/Main/Player/HealthUI");
		energyUI = GetNode<Label>("/root/Main/Player/EnergyUI");
		powerUI = GetNode<Label>("/root/Main/Player/PowerUI");
		levelUI = GetNode<Label>("/root/Main/Player/LevelUI");
		
		mainScene = GetParent() as Main;
		upgradeScene = ResourceLoader.Load<PackedScene>("res://scenes/Upgrade.tscn");
	}

	public override void _PhysicsProcess(double delta)
	{	
		DisplayUI();
		
		if (!dead && !isHit)
		{
			Vector2 velocity = Velocity;

			// get the horizontal and vertical input direction
			float directionX = Input.GetAxis("move_left", "move_right");
			float directionY = Input.GetAxis("move_up", "move_down");

			// set the velocity based on input
			velocity.X = directionX * Speed;
			velocity.Y = directionY * Speed;

			// handle sprite flipping based on horizontal direction
			if (directionX < 0)
			{
				animation.FlipH = true;
			}
			else if (directionX > 0)
			{
				animation.FlipH = false;
			}

			// play appropriate animations based on movement
			if (velocity == Vector2.Zero)
			{
				animation.Play("idle");
			}
			else
			{
				animation.Play("walk");
			}

			// update the velocity and move the player
			Velocity = velocity;
			MoveAndSlide();
		}
	}
	
	public void IncreaseHealth()
	{
		if (health < 3)
		{
			health += 1;
		}
	}

	public void IncreaseEnergy()
	{
		energy += 1;
		if (energy >= 10)
		{
			aoeAttack();
		}
	}
	
	public void IncreasePower()
	{
		power += 1;
		if (power >= 100)
		{
			LevelUp();
		}
	}
	
	private void DisplayUI()
	{
		healthUI.Text = "Health: " + health;
		energyUI.Text = "Energy: " + energy + "/10";
		powerUI.Text = "Power: " + power + "/100";
		levelUI.Text = "Level: " + level;
	}
	
	private void aoeAttack()
	{
		if (energy == 10)
		{
			energy -= 0;
			// implement attack
		}
	}
	
	private void LevelUp()
	{
		level += 1;
		power -= 100;
		
		// show upgrade scene
		var upgradeScreen = (Upgrade)upgradeScene.Instantiate();
		AddChild(upgradeScreen);
		upgradeScreen.ShowUpgradeScreen(this);
	}
	
	public void GetHit()
	{
		if (!isHit)  // ensure that the player is not already in the hit animation
		{
			// play sound
			soundPlayer = GetNode<AudioStreamPlayer>("Sounds/Damage");
			soundPlayer.Play();
		
			isHit = true;
			animation.Play("hit");
			health -= 1;
			if (health == 0)
			{
				if (extraLife)
				{
					extraLife = false;
					health = 3;
				}
				else
				{
					Die();
				}
			}
		}
	}
	
	private void Die()
	{
		dead = true;
		animation.Play("death");
	}
	
	private void OnAnimationFinished()
	{
		if (animation.Animation == "hit")
		{
			isHit = false;
			if (health == 0)
			{
				Die();
			}
		}
		else if (animation.Animation == "death")
		{
			float elapsedTime = mainScene.GetElapsedTime();
			GetTree().Root.GetNode("/root/Main").QueueFree();
			
			var gameOverScene = (PackedScene)ResourceLoader.Load("res://scenes/GameOver.tscn");
			var gameOverInstance = (Control)gameOverScene.Instantiate();
			GetTree().Root.AddChild(gameOverInstance);

			gameOverInstance.CallDeferred("SetElapsedTime", FormatTime(elapsedTime));
		}
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
}
