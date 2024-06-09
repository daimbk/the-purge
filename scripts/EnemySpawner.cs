using Godot;
using System;

public partial class EnemySpawner : Node
{
	[Export] public PackedScene EnemyScene;
	[Export] public float SpawnInterval = 2.0f;
	[Export] public float SpawnDistance = 500.0f;
	
	//public int numOfEnemies = 0;

	private Node2D player;
	private Timer spawnTimer;

	public override void _Ready()
	{
		// Initialize the player reference
		player = GetNode<Node2D>("/root/Main/Player");
		
		// Initialize and configure the spawn timer
		spawnTimer = new Timer();
		spawnTimer.WaitTime = SpawnInterval;
		spawnTimer.OneShot = false;
		spawnTimer.Connect("timeout", new Callable(this, nameof(SpawnEnemy)));
		AddChild(spawnTimer);
		spawnTimer.Start();
	}
	
	// for when an enemy dies
	//public void DecreaseEnemyNum()
	//{
		//numOfEnemies--;
	//}

	private void SpawnEnemy()
	{
		//if (numOfEnemies >= 50)
		//{
			//return;
		//}
		
		if (EnemyScene == null || player == null)
		{
			GD.PrintErr("Enemy scene or player reference is null!");
			return;
		}

		// Instance the enemy scene
		var enemyInstance = (CharacterBody2D)EnemyScene.Instantiate();
		AddChild(enemyInstance);

		// Calculate a random spawn position around the player
		Vector2 spawnPosition = player.GlobalPosition + new Vector2(SpawnDistance, 0).Rotated((float)GD.RandRange(0, 2 * Math.PI));
		enemyInstance.GlobalPosition = spawnPosition;
		
		//numOfEnemies++;
	}
}
