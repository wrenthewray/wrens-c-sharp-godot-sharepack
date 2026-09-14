using Godot;
using Shared.Entities.Cameras.Pivots;
using Shared.Managers.Cameras;

namespace Shared.Entities.Cameras;

/// <summary>
/// Basic class for an RTS style camera. Requires a tree structure like this:
/// <code>
/// Root
///     -> RTSCameraPivotEntity3D
///         -> SpringArm3D
///             -> RTSCameraEntity3D (this)
/// </code>
/// to function correctly. 
/// </summary>
[GlobalClass]
public partial class RTSCameraEntity3D : CameraEntity3D
{
    [Export] private RTSCameraPivotEntity3D cameraPivot;
    [Export] private SpringArm3D springArm3D;
    [Export] private float mouseSensitivity;
    [Export] private float zoomFactor = 1;
    /// <summary>
    /// The limit of zooming. Stored as degrees and converted to
    /// radians. X is the lowest angle allowed and Y is the largest
    /// angle allowed. 
    /// </summary>
    [Export (PropertyHint.Range, "1, 100")] private Vector2 zoomLimit = new(1, 10);

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if(!CameraEntity3DManager.CameraIsCurrentCamera(this))
            return;
        if(@event is InputEventMouseMotion inputEventMouseMotion)
        {
            if(Input.IsMouseButtonPressed(MouseButton.Middle))
                cameraPivot.Rotation = new Vector3(
                    cameraPivot.Rotation.X,
                    cameraPivot.Rotation.Y - inputEventMouseMotion.ScreenRelative.X * mouseSensitivity,
                    cameraPivot.Rotation.Z
                );
        }
    }
    public override void _Input(InputEvent @event)
    {
        if(Input.IsMouseButtonPressed(MouseButton.WheelUp))
            springArm3D.SpringLength -= zoomFactor * 0.01f;
        else if (Input.IsMouseButtonPressed(MouseButton.WheelDown))
            springArm3D.SpringLength += zoomFactor * 0.01f;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if(!CameraIsActive())
            return;
        springArm3D.SpringLength = Mathf.Clamp(springArm3D.SpringLength, zoomLimit.X, zoomLimit.Y);
    }
}