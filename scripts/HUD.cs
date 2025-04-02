using Godot;
using System;
using System.Threading.Tasks;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void ShowMessage(string messageText){
        var message = this.GetNode<Label>("Message");
        message.Text = messageText;
        message.Show();

        this.GetNode<Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver(){
        ShowMessage("Game Over");

        var messageTimer = this.GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        var message = this.GetNode<Label>("Message");
        message.Text = "Dodge the Creeps!";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        this.GetNode<Button>("StartButton").Show();
    }

    public void UpdateScoreLabel(int score){
        this.GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnStartButtonPressed(){
        this.GetNode<Button>("StartButton").Hide();
        this.EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout(){
        this.GetNode<Label>("Message").Hide();
    }
}
