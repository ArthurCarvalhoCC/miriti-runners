using Godot;
using System;

public partial class Parallax2d : Parallax2D
{
    public override void _Ready()
    {
		GameEvents.OnPlayerPaceChanged += pace => {
			Autoscroll = Vector2.Down * pace * 100;
		};
    }
}
