using Godot;


public partial class Node : Godot.Node
{
    public override void _Input(InputEvent @event)
    {
        GD.Print(@event.AsText());
    }
}

public partial class Player : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export]
    public int speed { get; set; } = 14;

    private Vector3 targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_down"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_up"))
        {
            direction.Z -= 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            // Setting the basis property will affect the rotation of the node.
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        // Ground velocity
        targetVelocity.X = direction.X * speed;
        targetVelocity.Z = direction.Z * speed;

        // Vertical velocity
        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            targetVelocity.Y -= 0;
        }

        // Moving the character
        Velocity = targetVelocity;
        MoveAndSlide();
    }
}