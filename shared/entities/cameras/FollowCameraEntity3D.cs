using Godot;
using Shared.Managers.Cameras;
using Shared.Resources.Data;

namespace Shared.Entities.Cameras;

/// <summary>
/// A camera that follows a target. 
/// </summary>
[GlobalClass]
public partial class FollowCameraEntity3D : CameraEntity3D
{
    [Export] protected Node3D target;
    public override void _Ready()
    {
        base._Ready();
        LookAtFromPosition(GlobalPosition, target.GlobalPosition, Vector3.Up);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if(!CameraIsActive())
            return;
        LookAtFromPosition(GlobalPosition, target.GlobalPosition, Vector3.Up);
    }
}