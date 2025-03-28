using Godot;
using System;

public partial class Player : Area2D
{
	[Godot.Signal]
	public delegate void HitEventHandler();

	[Godot.Export]
	public int Speed { get; set; } = 400; // pixels/second

	public Godot.Vector2 ScreenSize; // Size of the game window

	public void Start(Vector2 position) {
		this.Position = position;
		this.Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ScreenSize = this.GetViewportRect().Size;
		this.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// read input
		var vel = Vector2.Zero;

		if (Godot.Input.IsActionPressed("move_right"))
		{
			vel.X += 1;
		}
		if (Godot.Input.IsActionPressed("move_left"))
		{
			vel.X -= 1;
		}
		if (Godot.Input.IsActionPressed("move_down"))
		{
			vel.Y += 1;
		}
		if (Godot.Input.IsActionPressed("move_up"))
		{
			vel.Y -= 1;
		}

		var animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// select the animation
		if (vel.X != 0) {
			animatedSprite.Animation = "walk";
			animatedSprite.FlipV = false;
			animatedSprite.FlipH = vel.X < 0;
		} else if (vel.Y != 0) {
			animatedSprite.Animation = "up";
			animatedSprite.FlipH = false;
			animatedSprite.FlipV = vel.Y > 0;
		}

		// if we're moving start or stop the animation
		if (vel.Length() > 0)
		{
			vel = vel.Normalized() * Speed;
			animatedSprite.Play();
		} else {
			animatedSprite.Stop();
		}
		// move player
		this.Position += vel * (float)delta;
		this.Position = new Vector2(
			x: Godot.Mathf.Clamp(this.Position.X, 0, this.ScreenSize.X),
			y: Godot.Mathf.Clamp(this.Position.Y, 0, this.ScreenSize.Y)
		);

	}

	private void OnBodyEntered(Node2D body) {
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
