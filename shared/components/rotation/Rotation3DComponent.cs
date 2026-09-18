using Godot;

namespace Shared.Components.Rotation;

[GlobalClass, Icon("res://addons/at-icons/node3d/gimbal.svg")]
public partial class Rotation3DComponent : Node
{
    protected Node3D parent;
    [Export] public bool allowRotation = true;
    [Export] private bool allowRotationX = true;
    [Export] private bool allowRotationY = true;
    [Export] private bool lerp;
    [Export] private float lerpWeight = 1;

    /// <summary>
    /// The limit of x-axis tilt. Stored as degrees and converted to
    /// radians. X is the lowest angle allowed and Y is the largest
    /// angle allowed. 
    /// </summary>
    [Export] private bool limitTiltXAxis = false;
    [Export (PropertyHint.Range, "-90, 90")] private Vector2 xTiltLimit = new(-65f, 15);

    public override void _Ready()
    {
        base._Ready();

        parent = GetParent<Node3D>();
    }

    public void Rotate(float xFactor, float yFactor)
    {
        if(!allowRotation)
            return;
        var xRotation = parent.Rotation.X - xFactor;
        if(limitTiltXAxis)
            xRotation = Mathf.Clamp(xRotation, Mathf.DegToRad(xTiltLimit.X), Mathf.DegToRad(xTiltLimit.Y));
        
        var yRotation = parent.Rotation.Y - yFactor;
        var rotationVector = new Vector3(
            allowRotationX ? xRotation : parent.Rotation.X, 
            allowRotationY ? yRotation : parent.Rotation.Y,
            parent.Rotation.Z
        );

        if(lerp)
            parent.Rotation = parent.Rotation.Lerp(rotationVector, lerpWeight);
        else
            parent.Rotation = rotationVector;
    }

    public void ToggleRotation(bool toggle)
    {
        allowRotation = toggle;
    }
}