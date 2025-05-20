using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 8.0f;

	[Export]
	public float JumpVelocity = 12.0f;

	[Export]
	public float Gravity = 24.0f;

	[Export]
	public float Sensitivity = 0.002f;

	private Camera3D camera3D;
	private RayCast3D rayCast3D;

	public MaterialMenu MaterialMenu { get; set; } // Assigned externally, like in world.gd

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		camera3D = GetNode<Camera3D>("Camera3D");
		rayCast3D = camera3D.GetNode<RayCast3D>("RayCast3D");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			Rotation = new Vector3(
				Rotation.X,
				Rotation.Y - motion.Relative.X * Sensitivity,
				Rotation.Z
			);

			camera3D.Rotation = new Vector3(
				camera3D.Rotation.X - motion.Relative.Y * Sensitivity,
				camera3D.Rotation.Y,
				camera3D.Rotation.Z
			);

			var x = Mathf.Clamp(
				Mathf.RadToDeg(camera3D.Rotation.X),
				-70.0f,
				80.0f
			);
			camera3D.Rotation = new Vector3(
				Mathf.DegToRad(x),
				camera3D.Rotation.Y,
				camera3D.Rotation.Z
			);
		}

		if (@event.IsActionPressed("ui_tab"))
		{
			//GD.Print("Selected index is now:");
			MaterialMenu.CycleSelectionForward();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y -= Gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = 0;
			velocity.Z = 0;
		}

		// Left click block destroy
		if (Input.IsActionJustPressed("left_click") && rayCast3D.IsColliding())
		{
			var collider = rayCast3D.GetCollider();
			if (collider.HasMethod("DestroyBlock"))
			{
				var point = rayCast3D.GetCollisionPoint();
				var normal = rayCast3D.GetCollisionNormal();
				collider.Call("DestroyBlock", point - normal);
			}
		}

		// Right click block place
		if (Input.IsActionJustPressed("right_click") && rayCast3D.IsColliding())
		{
			var collider = rayCast3D.GetCollider();
			if (collider.HasMethod("PlaceBlock"))
			{
				var point = rayCast3D.GetCollisionPoint();
				var normal = rayCast3D.GetCollisionNormal();
				var tileId = (int)MaterialMenu.GetSelectedTileId();

				collider.Call("PlaceBlock", point + normal, tileId);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
