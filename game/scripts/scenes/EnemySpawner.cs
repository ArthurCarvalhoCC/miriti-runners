using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class EnemySpawner : Node2D
{
	[Export] public CollisionShape2D SpawnArea;
	[Export] public Array<PackedScene> Enemies;
	[Export] public Array<float> Chances;
	[Export] public float SpawnRate = 0.5f;
	[Export] public int MaxEnemies = 4;
	[Export] public float SpawnInterval = 1f;

	
	private Timer timer;
	private int currentEnemies = 0;
	private RandomNumberGenerator rng = new();

	public override void _Ready()
	{
		timer = new();
		AddChild(timer);
		timer.WaitTime = SpawnInterval;
		timer.OneShot = false;
		timer.Connect("timeout", Callable.From((Action)_on_timer_timeout));
		timer.Start();

		rng.Randomize();

		GameEvents.OnPlayerPaceChanged += pace => {
			timer.WaitTime = SpawnInterval / pace;
		};
	}

	private void _on_timer_timeout()
	{
		if (currentEnemies >= MaxEnemies) return;
		float chance = rng.Randf();
		for (int i = 0; i < Chances.Count; i++)
		{
			if (chance <= Chances[i])
			{
				PackedScene enemy = Enemies[i];
				Enemy2d newEnemy = enemy.Instantiate<Enemy2d>();
				AddChild(newEnemy);
				newEnemy.Position = GetRandomSpawnPosition();
				newEnemy.TreeExited += OnEnemyExited;
				newEnemy.StartMoving();
				currentEnemies++;
				return;
			}
		}
	}

	private Vector2 GetRandomSpawnPosition()
	{
		if (SpawnArea?.Shape is RectangleShape2D rect)
		{
			Vector2 half = rect.Size * 0.5f;
			return SpawnArea.Position + new Vector2(
				rng.RandfRange(-half.X, half.X),
				rng.RandfRange(-half.Y, half.Y)
			);
		}
		return Vector2.Zero;
	}

	private void OnEnemyExited()
	{
		currentEnemies--;
	}
}
