using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Test : ShapeCast3D
{
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready() // THIS IS GOOD CODE, FEEL FREE TO COPY ;)
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (Dictionary result in CollisionResult.Select(v => (Dictionary)v))
		{
			GD.Print(((Node3D)(GodotObject)result["collider"]).Name);
		}
	}
}
