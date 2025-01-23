using Godot;
using System;

public partial class tele_point : Node2D
{
	public bool isOverlapping = false;

	void _on_body_entered(Node2D buh) {
		isOverlapping = true;
	}

	void _on_body_exited(Node2D buh) {
		isOverlapping = false;
	}
}
