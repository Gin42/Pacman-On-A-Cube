using Godot;

public partial class Cube : MeshInstance3D
{

    public bool rotating = false;

    private Vector2 nextMousePosition;
    private Vector2 prevMousePosition;

    private double MOUSE_SENSITIVITY = 0.005;

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
	
		

