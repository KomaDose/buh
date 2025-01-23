using Godot;
using System;

public partial class camera2d : Camera2D
{
	player player;
	Path2D path;
	PathFollow2D pathfollow;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<player>("%Player");
		path = GetNode<Path2D>("%Path2D");
		pathfollow = GetNode<PathFollow2D>("%PathFollow2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float difference = path.Curve.GetPointPosition(0).X - player.Position.X;
		pathfollow.Progress = difference;
	}
}
