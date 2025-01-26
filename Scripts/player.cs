using Godot;
using System;

public partial class player : CharacterBody2D
{
	public float Speed = 110.0f;
	public float JumpVelocity = -250.0f;
	Vector2 direction = new(-1, 0);
	float coyoteTime = 0.1f;
	float coyoteTimeCounter;
	float jumpBufferTime = 0.1f;
	float jumpBufferTimeCounter;
	
	Node2D startPos;

	Sprite2D sprite;

	CollisionPolygon2D col;

	light_rays lr;

	[Export] Node2D telePoint;
	[Export] PathFollow2D teleRange;
	[Export] Sprite2D teleSprite;
	[Export] Path2D path;
	Vector2 teleportDir;

	bool cancled = false;

	tele_point tpScript;

	PackedScene smoke = GD.Load<PackedScene>("res://Scenes/smoke.tscn");

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready() {
		startPos = GetNode<Node2D>("%Start Pos");
		sprite = GetNode<Sprite2D>("Sprite2D");
		col = GetNode<CollisionPolygon2D>("Collision");
		lr = GetNode<light_rays>("%Light Rays");
		tpScript = GetNode<tele_point>("Path2D/TeleRange/Area2D");
		this.Position = startPos.Position;
		sprite.FlipH = true;
		col.Scale = new Vector2(-1, 1);
		teleSprite.FlipH = true;
		path.Curve.SetPointPosition(1, new Vector2(-65, 4));
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y += gravity * (float)delta;
		}

		//Coyote Time
		/*
		if (IsOnFloor()) {
			coyoteTimeCounter = coyoteTime;
		}
		else {
			coyoteTimeCounter -= (float)delta;
		}

		//Jump Buffer
		if (Input.IsActionJustPressed("jump")) {
			jumpBufferTimeCounter = jumpBufferTime;
		}
		else {
			jumpBufferTimeCounter -= (float)delta;
		}
			
		// Handle Jump.
		if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f) {
			velocity.Y = JumpVelocity;
			coyoteTimeCounter = 0f;
			jumpBufferTimeCounter = 0f;
		}
		*/
		
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		velocity = new Vector2(direction.X * Speed, velocity.Y);
		
		//Sprite
		if (direction == Vector2.Left) {
			sprite.FlipH = true;
			col.Scale = new Vector2(-1, 1);
			teleSprite.FlipH = true;
		}
		else if (direction == Vector2.Right) {
			sprite.FlipH = false;
			col.Scale = new Vector2(1, 1);
			teleSprite.FlipH = false;
		}

		teleSprite.GlobalPosition = teleSprite.GlobalPosition.Lerp(telePoint.GlobalPosition + new Vector2(0, -4), (float)delta * 6f);

		//Teleportation
		if (Input.IsActionJustPressed("left")) {
			if (teleRange.ProgressRatio > 0 && teleportDir != Vector2.Left) {
				teleRange.ProgressRatio -= 0.5f;
			}
			else {
				path.Curve.SetPointPosition(1, new Vector2(-65, 4));
				teleportDir = Vector2.Left;
				teleRange.ProgressRatio += 0.5f;
			}
		}
		else if (Input.IsActionJustPressed("right")) {
			if (teleRange.ProgressRatio > 0 && teleportDir != Vector2.Right) {
				teleRange.ProgressRatio -= 0.5f;
			}
			else {
				path.Curve.SetPointPosition(1, new Vector2(65, 4));
				teleportDir = Vector2.Right;
				teleRange.ProgressRatio += 0.5f;
			}
		}
		else if (Input.IsActionJustPressed("up")) {
			if (teleRange.ProgressRatio > 0 && teleportDir != Vector2.Up) {
				teleRange.ProgressRatio -= 0.5f;
			}
			else {
				path.Curve.SetPointPosition(1, new Vector2(0, -65));
				teleportDir = Vector2.Up;
				teleRange.ProgressRatio += 0.5f;
			}
		}
		else if (Input.IsActionJustPressed("down")) {
			if (teleRange.ProgressRatio > 0 && teleportDir != Vector2.Down) {
				teleRange.ProgressRatio -= 0.5f;
			}
			else {
				path.Curve.SetPointPosition(1, new Vector2(0, 65));
				teleportDir = Vector2.Down;
				teleRange.ProgressRatio += 0.5f;
			}
		}
		/*
		if (Input.IsActionJustPressed("jump")) {
			if (teleRange.ProgressRatio > 0 && !tpScript.isOverlapping) {
				this.Position = telePoint.GlobalPosition;
				teleRange.ProgressRatio = 0f;
				teleSprite.Position = Vector2.Zero;
			}
		}
		*/
		//Darkness
		if (Input.IsActionJustPressed("jump")) {
			if (teleRange.ProgressRatio > 0) {
				SpawnSmoke();
			}
		}
		Velocity = velocity;
		MoveAndSlide();
	}

	public void SpawnSmoke() {
		StaticBody2D instance = (StaticBody2D)smoke.Instantiate();
		AddSibling(instance);
		instance.Position = telePoint.GlobalPosition;
	}

	public void Death() {
		this.Position = startPos.Position;
	}
}
