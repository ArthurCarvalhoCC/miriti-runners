using Godot;
using System;

public partial class GameEvents : Node
{
    public static GameEvents Instance { get; set; }

    public override void _Ready()
    {
        Instance = this;
    }

    private GameEvents() { }

    public static event Action<Node> OnPlayerDied;
    public static void TriggerPlayerDied(Node source)
    {
        OnPlayerDied?.Invoke(source);
    }

    public static event Action<float> OnPlayerPaceChanged;
    public static void TriggerPlayerPaceChanged(float value)
    {
        OnPlayerPaceChanged?.Invoke(value);
    }
}
