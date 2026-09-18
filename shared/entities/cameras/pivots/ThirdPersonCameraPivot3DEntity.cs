using Godot;
using Shared.Components.Follow;
using System;

namespace Shared.Entities.Cameras.Pivots;
/// <summary>
/// A pivot for a 3rd person camera. This pivot allows the camera to move around the scene
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/hinge_joint.svg")]
public partial class ThirdPersonCameraPivot3DEntity : Node3D
{
    [Export] protected Node3D target;
    [Export] private Follow3DComponent follow3DComponent;

    public override void _Ready()
    {
        base._Ready();

        follow3DComponent.target = target;
    }

}
