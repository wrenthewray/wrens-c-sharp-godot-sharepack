using Godot;
using Shared.Resources;
using System;

namespace Shared.Entities.Cameras.Pivots;

/// <summary>
/// A pivot for a RTS camera. The camera attaches to this pivot, which moves around the scene
/// and rotates the camera alongside it. It is meant to be used with a <see cref="RTSCameraEntity3D"/>, 
/// which controls this entity's rotation.
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/hinge_joint.svg")]
public partial class RTSCameraPivotEntity3D : Node3D
{
    private Vector2 direction = Vector2.Zero;
    /// <summary>
    /// The speed at which this pivot can move around the scene.
    /// </summary>
    [Export] protected float moveSpeed = 15f;
    /// <summary>
    /// The angle at which the camera is rotated around the X axis. This is used to give the camera a top down view of the scene.
    /// </summary>
    [Export (PropertyHint.Range, "-75,15,1")] protected int xAngle = -45; 
    public override void _Ready()
    {
        base._Ready();

        RotateX(Mathf.DegToRad(xAngle));
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        direction = Input.GetVector(
            InputAction.MoveLeft,
            InputAction.Right,
            InputAction.Up,
            InputAction.Down
        );

        Position += new Vector3(
            direction.X * (float) delta * moveSpeed,
            0f,
            direction.Y * (float) delta * moveSpeed
        ).Rotated(Vector3.Up, Rotation.Y);
    }
}