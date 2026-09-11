using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 1000f;

	[Export]
	public float Acceleration { get; set; } = 1000f;

	[Export]
	public float Friction { get; set; } = 2000f;

	private float currentSpeedX = 0f;

	public override void _Ready()
	{
		GameEvents.OnPlayerDied += source =>
        {
			GD.Print("Player died");
		};
	}

	public void BoostPlayer(float value) => currentSpeedX += value;

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("sprint"))
			GameManager.Instance.AcceleratePlayerPace(.05f);
		else
		{
			if (GameManager.Instance.GetPlayerPace() > 1)
				GameManager.Instance.AcceleratePlayerPace(-.1f);
			else
				GameManager.Instance.ResetPlayerPace();
		}

		float direction = Input.GetAxis("left", "right");
		float targetSpeed = direction * Speed;
		float deltaSpeed = (float)delta;

		if (direction != 0)
		{
			currentSpeedX = Mathf.MoveToward(currentSpeedX, targetSpeed, Acceleration * deltaSpeed * 2);
		}
		else
		{
			currentSpeedX = Mathf.MoveToward(currentSpeedX, 0, Friction * 2 * deltaSpeed);
		}
		var vel = Velocity;

		vel.Y = 0; // mantenha Velocity.Y se você usar gravidade
		vel.X = currentSpeedX;

		Velocity = vel;
		MoveAndSlide();
	}
}
