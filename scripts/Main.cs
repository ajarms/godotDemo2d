using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene {get; set;}

    private int _score;

    public override void _Ready()
    {
        // NewGame();
    }

    public void GameOver(){
        this.GetNode<Timer>("MobTimer").Stop();
        this.GetNode<Timer>("ScoreTimer").Stop();

        this.GetNode<AudioStreamPlayer>("Music").Stop();
        this.GetNode<AudioStreamPlayer>("DeathSound").Play();

        this.GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame(){
        _score = 0;

        var playerInstance = this.GetNode<Player>("Player");
        var startPos = this.GetNode<Marker2D>("StartPosition");

        var hud = this.GetNode<HUD>("HUD");
        hud.UpdateScoreLabel(_score);
        hud.ShowMessage("Get Ready!");

        this.GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

        playerInstance.Start(startPos.Position);
        
        this.GetNode<AudioStreamPlayer>("Music").Play();

        this.GetNode<Timer>("StartTimer").Start();
    }

    public void OnMobTimerTimeout(){
        // TODO
        /*
        spawn mob at random location along the mob spawn path
        set a random speed for the mob between 150 and 250
        */

        Mob mobInstance = MobScene.Instantiate<Mob>();

        var mobSpawnLocation = this.GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        // the path is clockwise around the perimeter of the screen,
        // so turn clockwise perpendicular to the path to face the interior of the screen
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        mobInstance.Position = mobSpawnLocation.Position;

        // TODO this is taken from the tutorial, but it has an edge case:
        // if the mob is spawned close to a corner it's perpendicular heading will be closely
        // parallel to the screen edge, and if this randomly tilts it in the wrong direction
        // it will immediately travel off of the screen after being spawned.
        // To avoid this, it could either detect corners, or only allow for a negative tilt
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);

        var velocity = new Vector2((float)GD.RandRange(150, 250), 0).Rotated(direction);
        mobInstance.LinearVelocity = velocity;

        this.AddChild(mobInstance);
    }

    public void OnStartTimerTimeout(){
        this.GetNode<Timer>("MobTimer").Start();
        this.GetNode<Timer>("ScoreTimer").Start();
    }

    public void OnScoreTimerTimeout(){
        // score based on how long the player survives
        _score++;
        this.GetNode<HUD>("HUD").UpdateScoreLabel(_score);
    }
}
