using Godot;
using System;

public partial class Cube : StaticBody3D
{

	 public bool rotating = false;

    private Vector2 nextMousePosition;
    private Vector2 prevMousePosition;

    private double MOUSE_SENSITIVITY = 0.005;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("rotate")) {
            rotating = true;
            prevMousePosition = GetViewport().GetMousePosition();
            GD.Print($"Get ready to fly");
        }
        if (Input.IsActionJustReleased("rotate")){
            rotating = false;
            GD.Print($"Thank you for flying with raynair");
        }
        if (rotating){
            nextMousePosition = GetViewport().GetMousePosition();
            Vector2 mdelta = nextMousePosition - prevMousePosition;
            // horizontal -> yaw (Y), vertical -> pitch (X)
            RotateY((float)(mdelta.X * MOUSE_SENSITIVITY)); // sign depends on desired direction
            RotateX((float)(mdelta.Y * MOUSE_SENSITIVITY));
            prevMousePosition = nextMousePosition;
        }
	}
}
