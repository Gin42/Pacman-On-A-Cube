using Godot;
using System;

public partial class PacmanTest : RigidBody3D
{

	private Vector3 gravityDirection;
	private int moveForce = 5;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CalcGravityDirection();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Move();
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		//base._IntegrateForces(state);
		Walk(state);
	}

	private void CalcGravityDirection()
	{
		gravityDirection = (GetParent().GetNode<Node3D>("CubeTest").GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		GD.Print("Gravity Direction:", gravityDirection);
	}

	private void Walk(PhysicsDirectBodyState3D state)
	{

		Transform3D transform = state.Transform;
		transform.Basis.Y = -gravityDirection;
		state.Transform = transform;
	}

	private void Move()
	{
		if (Input.IsActionPressed("move_right"))
		{
			AddConstantCentralForce(moveForce * GlobalTransform.Basis.X);
		}
		if (Input.IsActionPressed("move_left"))
		{
			AddConstantCentralForce(moveForce * -GlobalTransform.Basis.X);
		}
		if (Input.IsActionPressed("move_back"))
		{
			AddConstantCentralForce(moveForce * GlobalTransform.Basis.Z);
		}
		if (Input.IsActionPressed("move_forward"))
		{
			AddConstantCentralForce(moveForce * -GlobalTransform.Basis.Z);
		}
	}

}
