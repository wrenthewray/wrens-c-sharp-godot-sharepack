using Godot;
using Shared.Entities.Cameras.Pivots;
using Shared.Managers.Cameras;

namespace Shared.Entities.Cameras;

/// <summary>
/// Basic class for a 3rd person camera. Requires a tree structure like this:
/// <code>
/// TargetEntity3D
///     -> CameraPivotEntity3D
///         -> SpringArm3D
///             -> ThirdPersonCameraEntity3D (this)
/// </code>
/// to function correctly. 
/// </summary>
[GlobalClass]
public partial class ThirdPersonCameraEntity3D : CameraEntity3D
{
    [Export] private ThirdPersonCameraPivotEntity3D cameraPivot;
    [Export] private float mouseSensitivity;
    /// <summary>
    /// The limit of up-down tilt. Stored as degrees and converted to
    /// radians. X is the lowest angle allowed and Y is the largest
    /// angle allowed. 
    /// </summary>
    [Export (PropertyHint.Range, "-90, 90")] private Vector2 tiltLimit = new(-65f, 15);

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if(!CameraIsActive())
            return;
            
        if(@event is InputEventMouseMotion inputEventMouseMotion)
        {
            cameraPivot.Rotation = new Vector3(
                Mathf.Clamp(
                    cameraPivot.Rotation.X - inputEventMouseMotion.ScreenRelative.Y * mouseSensitivity, 
                    Mathf.DegToRad(tiltLimit.X),
                    Mathf.DegToRad(tiltLimit.Y)
                ),
                cameraPivot.Rotation.Y - inputEventMouseMotion.ScreenRelative.X * mouseSensitivity,
                cameraPivot.Rotation.Z
            );
        }
    }
}