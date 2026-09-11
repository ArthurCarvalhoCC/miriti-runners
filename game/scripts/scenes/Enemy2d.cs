using Godot;
using System;

[GlobalClass]
public partial class Enemy2d : CharacterBody2D
{
	[Export]
	public bool Moving = false;
	[Export]
	public Area2D CollisionArea;

	public void StartMoving() => Moving = true;
	public void StopMoving() => Moving = false;

	[Export]
	public int Speed = 100;

	public int direction = 0;
	public int SpeedX = 100;
	private const int LEFT = -1;
	private const int NONE = 0;
	private const int RIGHT = 1;

	private RandomNumberGenerator rng = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionArea.Connect("area_entered", Callable.From((Action<Area2D>)CollisionTouched));

		rng.Randomize();
		direction = rng.RandiRange(LEFT, RIGHT);

		GameEvents.OnPlayerDied += s =>
		{
			if (s is Enemy2d) s.QueueFree();
		};
	}

	public void CollisionTouched(Area2D area)
	{
		if (area.Name == "Player") GameEvents.TriggerPlayerDied(this);
	}
	public override void _ExitTree()
	{
		CollisionArea.Disconnect("area_entered", Callable.From((Action<Area2D>)CollisionTouched));
	}

    public override void _PhysicsProcess(double delta)
    {
		var vec = new Vector2();
		if (Moving) {
			vec += Transform.Y * Speed * (float)delta * GameManager.Instance.GetPlayerPace() * 100;
			vec += Transform.X * SpeedX * direction * (float)delta * 10;
		}
		if (Position.Y > (GetViewportRect().Size.Y * 1.2)) QueueFree();

		Velocity = vec;
		MoveAndSlide();
    }
}
