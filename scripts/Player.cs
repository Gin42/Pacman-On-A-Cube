using System;
using Godot;

/*public partial class Player : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export]
    public int Speed { get; set; } = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

    @onReady 

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
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_forward"))
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
        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        // Vertical velocity
        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        // Moving the character
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
*/

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float Radius = 0.5f;
    [Export] public float AlignSpeed = 10.0f;

    private Vector3 inputDir = Vector3.Zero;

    private ShapeCast3D _groundCast;

    private Vector3 _groundPos;
    private Vector3 _groundUp = Vector3.Up;

    public override void _Ready()
    {
        _groundCast = GetNode<ShapeCast3D>("GroundDetector");
    }

    public override void _PhysicsProcess(double delta)
    {
        float d = (float)delta;

        bool onGround = UpdateGround();

        if (onGround)
        {
            AlignToGround(d);
        }
        else
        {
            // fallback gravity
            Velocity += -Transform.Basis.Y * 9.8f * d;
        }

        inputDir = GetInputDirection();
        GD.Print("Mi direziono a: ", inputDir);
        Velocity = inputDir * Speed;

        MoveAndSlide();
    }

    private bool UpdateGround()
    {
        _groundCast.ForceShapecastUpdate();

        if (_groundCast.IsColliding())
        {
            _groundPos = _groundCast.GetCollisionPoint(0);
            _groundUp = _groundCast.GetCollisionNormal(0);
            return true;
        }

        return false;
    }

    private void AlignToGround(float delta)
    {
        // Snap position to surface
        Vector3 targetPos = _groundPos + _groundUp * Radius;
        GlobalPosition = GlobalPosition.Lerp(targetPos, delta * 10.0f);

        // Align "up" direction
        Vector3 currentUp = Transform.Basis.Y;

        Vector3 axis = currentUp.Cross(_groundUp);
        float angle = currentUp.AngleTo(_groundUp);

        if (angle > 0.001f && axis.Length() > 0.0001f)
        {
            axis = axis.Normalized();
            Basis rot = new Basis(axis, angle * AlignSpeed * delta);

            Basis newBasis = rot * Transform.Basis;
            newBasis = newBasis.Orthonormalized();

            Transform = new Transform3D(newBasis, Transform.Origin);
        }
    }

    private Vector3 GetInputDirection()
    {
        Vector3 dir = Vector3.Zero;

        if (Input.IsActionPressed("move_forward"))
            dir -= Transform.Basis.Z;

        if (Input.IsActionPressed("move_back"))
            dir += Transform.Basis.Z;

        if (Input.IsActionPressed("move_left"))
            dir -= Transform.Basis.X;

        if (Input.IsActionPressed("move_right"))
            dir += Transform.Basis.X;

        return dir.Normalized();
    }
}