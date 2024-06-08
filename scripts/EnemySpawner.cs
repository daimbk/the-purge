using Godot;
using System;

public partial class EnemySpawner : Node
{
	[Export] public PackedScene EnemyScene;
	[Export] public float SpawnInterval = 2.0f;
	[Export] public float SpawnDistance = 500.0f;
	
	private int numOfEnemies = 0;

	private Node2D player;

	public override void _Ready()
	{
		// Initialize the player reference
		player = GetNode<Node2D>("/root/Main/Player");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (numOfEnemies < 10)
		{
			SpawnEnemy();
			numOfEnemies++;
		}
	}

	private void SpawnEnemy()
	{
		GD.Print("Enemy Spawned");
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
	}
}
