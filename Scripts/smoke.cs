using Godot;
using System;

public partial class smoke : StaticBody2D
{
	[Export] Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

	}

	public void _on_timer_timeout() {
		this.QueueFree();
	}
}
