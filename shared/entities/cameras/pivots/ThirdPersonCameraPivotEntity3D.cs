using Godot;
using System;

namespace Shared.Entities.Cameras.Pivots;
/// <summary>
/// A pivot for a 3rd person camera. This pivot allows the camera to move around the scene
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/hinge_joint.svg")]
public partial class ThirdPersonCameraPivotEntity3D : Node3D
{
    [Export] protected Node3D target;
    [Export] protected Vector3 offset;
    [Export] protected PivotFollowType followType = PivotFollowType.Smooth;
    [Export] protected float followSpeed = 4.0f;

    public override void _Process(double delta)
    {
        base._Process(delta);
    
        switch(followType)
        {
            case PivotFollowType.Simple:
                Position = target.Position + offset;
            break;
            case PivotFollowType.Smooth:
                float weight = 1f - Mathf.Exp(-followSpeed * (float)delta);
                Position = Position.Lerp(target.Position + offset, weight);
            break;
        }
    }
}
public enum PivotFollowType
{
    Simple,
    Smooth,
    
}