using Godot;
using System;
using System.Diagnostics;

public partial class player : CharacterBody2D
{
	public float Speed = 80.0f;
	Vector2 direction = new(-1, 0);

	public float JumpVelocity = -200.0f;
	float coyoteTime = 0.1f;
	float coyoteTimeCounter;
	float jumpBufferTime = 0.1f;
	float jumpBufferTimeCounter;

	bool isDashing = false;
	float dashSpeed = 200f;
	bool canDash = true;
	float dashTime = 0.2f;
	float dashTimeCounter;
	float dashCooldownTime = 1f;
	float dashCooldownTimeCounter;

	Vector2 facingDirection;
	
	Node2D startPos;

	Sprite2D sprite;

	CollisionPolygon2D col;

	bool cancled = false;

	PackedScene smoke = GD.Load<PackedScene>("res://Scenes/smoke.tscn");

	[Export] AnimationPlayer ap;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready() {
		startPos = GetNode<Node2D>("%Start Pos");
		sprite = GetNode<Sprite2D>("Sprite2D");
		col = GetNode<CollisionPolygon2D>("Collision");
		this.Position = startPos.Position;
		sprite.FlipH = true;
		col.Scale = new Vector2(-1, 1);
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		//Gravity.
		if (isDashing) {
			velocity.Y = 0;
		}
		else {
			if (!IsOnFloor()) {
				velocity.Y += gravity * (float)delta;
			}
		}
		

		//Coyote Time
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

		//Variable Jump Height
		if (Input.IsActionJustReleased("jump") && Velocity.Y < 0) {
			velocity.Y *= 0.5f;
		}
		
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		velocity = new Vector2(direction.X * Speed, velocity.Y);
		
		//Sprite
		if (direction == Vector2.Left) {
			sprite.FlipH = true;
			col.Scale = new Vector2(-1, 1);
		}
		else if (direction == Vector2.Right) {
			sprite.FlipH = false;
			col.Scale = new Vector2(1, 1);
		}


		if (direction == Vector2.Left) {
			facingDirection = Vector2.Left;
		}
		else if (direction == Vector2.Right) {
			facingDirection = Vector2.Right;
		}

		//Shoot

		
		//Dash
		if (Input.IsActionJustPressed("action_z") && canDash) {
			dashTimeCounter = dashTime;
			dashCooldownTimeCounter = dashCooldownTime;
			canDash = false;
		}
		else {
			dashTimeCounter -= (float)delta;
			dashCooldownTimeCounter -= (float)delta;
		}
		
		if (dashCooldownTimeCounter <= 0f) {
			canDash = true;
		}

		if (dashTimeCounter > 0f) {
			isDashing = true;
			velocity.X = dashSpeed * facingDirection.X;
		}
		else {
			isDashing = false;
		}

		//Smoke
		if (Input.IsActionJustPressed("action_x")) {
			SpawnSmoke();
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void SpawnSmoke() {
		StaticBody2D instance = (StaticBody2D)smoke.Instantiate();
		AddSibling(instance);
		instance.Position = this.GlobalPosition;
	}

	public void Death() {
		this.Position = startPos.Position;
	}
}
