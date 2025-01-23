using Godot;
using System;
using System.Linq;

public partial class light_rays : Node2D
{
	CharacterBody2D player;
	player p;
	RayCast2D[] ray;
	float distance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		player = GetNode<CharacterBody2D>("%Player");
		p = GetNode<player>("%Player");
		ray = GetChildren().OfType<RayCast2D>().ToArray();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
		LookAt(player.Position);
		distance = player.Position.DistanceTo(this.GlobalPosition);

		foreach (var Index in ray) {
			Index.TargetPosition = new Vector2(Index.TargetPosition.X, distance + 5);
			
			if (Index.IsColliding() && Index.GetCollider() is player) {
				Index.Show();
				p.Death();
			}
			else {
				Index.Hide();
			}
		}
	}
}
