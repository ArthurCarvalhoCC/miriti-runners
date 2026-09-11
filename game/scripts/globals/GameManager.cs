using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }

    private float playerPace = 1;
    public void SetPlayerPace(float value) { 
        if (value == playerPace) return;
        GameEvents.TriggerPlayerPaceChanged(value); 
        playerPace = value; 
    }
    public void AcceleratePlayerPace(float value) => SetPlayerPace(playerPace + value);
    public void ResetPlayerPace() => playerPace = 1;
    public float GetPlayerPace() => playerPace;

}
