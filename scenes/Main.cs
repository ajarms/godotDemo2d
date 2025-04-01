using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene {get; set;}

    private int _score;

    public void GameOver(){
        this.GetNode<Timer>("MobTimer").Stop();
        this.GetNode<Timer>("ScoreTimer").Stop();
    }

    public void NewGame(){
        _score = 0;

        var playerInstance = this.GetNode<Player>("Player");
        var startPos = this.GetNode<Marker2D>("StartPosition");
        playerInstance.Start(startPos.Position);

        this.GetNode<Timer>("StartTimer").Start();
    }

    public void OnMobTimerTimeout(){
        // TODO
        /*
        spawn mob at random location along the mob spawn path
        set a random speed for the mob between 150 and 250
        */
    }

    public void OnStartTimerTimeout(){
        this.GetNode<Timer>("MobTimer").Start();
        this.GetNode<Timer>("ScoreTimer").Start();
    }

    public void OnScoreTimerTimeout(){
        // score based on how long the player survives
        _score++;
    }
}
